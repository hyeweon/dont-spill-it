using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    SearchBrick,
    SearchLadderZone,
    Move,
    MoveLadderZone,
    SetLadder,
}

public class NonPlayer : ladderStack
{
    // 상태 전환을 통해 NonPlayer의 상태를 관리한다.

    /* 탐색
     * 내 범위 내로 들어온 바닥에 놓여진 사다리로 랜덤하게 이동한다.
     * - 레이어 처리를 통해 습득한 사다리, 놓여진 사다리 등은 탐지하지 않게 구분한다.
     */

    /* 습득
     * 인터페이스나 abstract로 받아서 사다리가 쌓이는 부분 관리
     */

    /* 사다리 설치
     * 일정 개수 이상의 사다리가 쌓이면 가까운 설치 구역으로 이동하여 설치한다.
     * 설치를 하면 습득한 사다리가 하나씩 감소하고, 해당 구역에 사다리가 추가된다.
     * 사다리가 없을 때 까지 내려간다.
     * 사다리가 없으면 다시 위로 올라가서 탐색 상태가 된다.
     * 설치된 사다리의 자신의 사다리가 다 쌓여져 있는지 체크하고, 다 쌓이면 아래층으로 내려간다. 
     */

    [SerializeField] private NPCState nPCState;
    [SerializeField] private float speed = 3;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private int brickCount = 0;
    [SerializeField] private Animator npcAnimator;

    [SerializeField] private Rigidbody npcRB;
    [SerializeField] private Collider npcColider;

    [SerializeField] private Transform targetTr;
    [SerializeField] private Transform headTr;

    [SerializeField] private putLadderPooling[] myLadderZone;
    // [SerializeField] private putLadderPooling[] myLadderZone2;
    private int myLadderCount = 0;
    [SerializeField] private Vector3 targetPos = Vector3.zero;
    [SerializeField] private Vector3 boxScale;


    private int putZoneLayer;
    private int floorLayer;
    private int gimmick;
    private bool isSetLadder = false;

    public Action GetMoreAction { get; private set; }

    private void Start()
    {
        targetLayer_ray = 1 << targetLayer_collision;

        putZoneLayer = LayerMask.NameToLayer("putZone");
        floorLayer = LayerMask.NameToLayer("Floor");
        gimmick = LayerMask.NameToLayer("gimmick");

        backpos.localPosition = setBackPos();

        SwitchState(NPCState.SearchBrick);

        GetMoreAction = GetMoreBrick;
    }

    private void SwitchState(NPCState state)
    {
        nPCState = state;

        switch (state)
        {
            case NPCState.SearchBrick:
                {
                    targetPos = Vector3.zero;
                    StartCoroutine(SearchbyLayer(targetLayer_ray)); break;
                }
            case NPCState.SearchLadderZone:
                {
                    targetPos = Vector3.zero;
                    StartCoroutine(SearchbyLayer(1 << putZoneLayer)); break;
                }
            case NPCState.Move:
                {
                    StartCoroutine(MoveToTarget()); break;
                }
            case NPCState.MoveLadderZone:
                {
                    StartCoroutine(MoveToLadderZone()); break;
                }
            case NPCState.SetLadder:
                {
                    targetPos = Vector3.zero;
                    StartCoroutine(LadderSlotChk()); break;
                }
        }
    }

    private IEnumerator SearchbyLayer(int layer)
    {
        float maxTime = 0;
        while (targetPos == Vector3.zero)
        {
            yield return null;
            maxTime += Time.deltaTime;

            // 일정 시간 이상 더 찾지 못하면 사다리로 이동
            if (maxTime >= 5)
            {
                ResetTarget();
                SwitchState(NPCState.MoveLadderZone);
                yield break;
            }

            Collider[] cols = Physics.OverlapBox(transform.position, boxScale, Quaternion.identity, layer);
            if (cols.Length > 0)
            {
                int randombrick = UnityEngine.Random.Range(0, cols.Length);
                if (cols[randombrick].transform.position.y < headTr.position.y)
                {
                    // print($"target: {cols[randombrick].transform.name} y: {cols[randombrick].transform.position.y} headY: {headTr.position.y}");
                    targetTr = cols[randombrick].transform;
                    targetPos = cols[randombrick].transform.position;

                    SwitchState(NPCState.Move);
                    yield break;
                }
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, boxScale);
    //}

    private IEnumerator MoveToTarget()
    {
        int targetLadderCount = UnityEngine.Random.Range(2, 6);
        while (nPCState == NPCState.Move)
        {
            yield return null;
            npcAnimator.SetFloat("MoveSpeed", 1);
            transform.position = Vector3.MoveTowards(transform.position, targetTr.position, speed * Time.deltaTime);// * speed * Time.deltaTime;
            // 사다리로 가고 있는데 이게 왜 동작하지?

            var angle = GetRotAngle(targetPos);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        targetTr = null;
    }

    bool isMoveToLadder = false;
    private IEnumerator MoveToLadderZone()
    {
        if (isMoveToLadder == true)
            yield break;

        isMoveToLadder = true;

        targetPos = myLadderZone[myLadderCount].transform.position;

        var angle = GetRotAngle(targetPos);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        while (nPCState == NPCState.MoveLadderZone)
        {
            yield return null;
            npcAnimator.SetFloat("MoveSpeed", 1);

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (isSetLadder == false && Vector3.Distance(targetPos, transform.position) <= 1.3f)
            {
                SwitchState(NPCState.SetLadder);
                yield break;
            }
        }
    }

    // WaitForSeconds wfs05 = new WaitForSeconds(0.5f);
    private IEnumerator LadderSlotChk()
    {
        npcAnimator.SetFloat("MoveSpeed", 0);
        isSetLadder = true;

        while (nPCState == NPCState.SetLadder)
        {
            yield return null;
            if (slot.Count <= 0)
            {
                ResetTarget();

                brickCount = 0;
                isSetLadder = false;
                isMoveToLadder = false;
                yield break;
            }
        }

        ResetTarget();
        brickCount = 0;
        isSetLadder = false;
        isMoveToLadder = false;
    }

    private IEnumerator GoingDownLadder()
    {
        npcAnimator.SetFloat("MoveSpeed", 0);
        npcAnimator.SetBool("Ladder", true);
        npcAnimator.SetInteger("PosY", -1);

        while (nPCState == NPCState.SetLadder)
        {
            yield return null;
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
        }

        npcAnimator.SetBool("Ladder", false);
        npcAnimator.SetInteger("PosY", 0);
        isCrossing = false;
    }

    private IEnumerator GoingFowardLadder()
    {
        transform.position = myLadderZone[myLadderCount].transform.position;
        transform.rotation = Quaternion.AngleAxis(90, Vector3.up);

        while (nPCState == NPCState.SetLadder)
        {
            yield return null;
            npcAnimator.SetFloat("MoveSpeed", 1);
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
        }

        isCrossing = false;
    }

    private float GetRotAngle(Vector3 _targetPos)
    {
        var tempDir = _targetPos - transform.position;
        var angle = Mathf.Atan2(tempDir.x, tempDir.z) * Mathf.Rad2Deg;
        return angle;
    }

    //기믹에 닿았을시 일정시간 멈추기
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gimmick)
        {
            StartCoroutine(stopMove1sec());
        }
    }
    bool isCrossing = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer_collision)
        {
            CollisionWithSitPeople(other.gameObject);
        }

        if (other.gameObject.layer == putZoneLayer)
        {
            myLadderZone[myLadderCount].setLadderAction(this, (isDown) =>
                {
                    isCrossing = true;

                    SwitchState(NPCState.SetLadder);
                    ResetTarget();
                    DownLadderSetting(true);

                    // down
                    if (isDown)
                        StartCoroutine(GoingDownLadder());// DownLadder();
                    // foward
                    else
                        StartCoroutine(GoingFowardLadder());// CrossTheLadder();
                });
        }

        // 아래층으로 내려왔을 때 
        if (other.gameObject.layer == floorLayer)
        {
            npcColider.isTrigger = false;
            brickCount = 0;

            SwitchState(NPCState.SearchBrick);

            myLadderCount = 1;
            isSetLadder = false;
            isMoveToLadder = false;
            npcRB.useGravity = true;

            npcAnimator.SetInteger("PosY", 0);
            npcAnimator.SetBool("Ladder", false);
        }
    }

    private void CollisionWithSitPeople(GameObject sitPeople)
    {


        sitPeople.layer = LayerMask.NameToLayer("Hang");

        int targetLadderCount = UnityEngine.Random.Range(2, 6);
        brickCount++;

        stackBack(sitPeople, this.transform);
        // isMoveToLadder = false;

        if (isMoveToLadder == true)
            return;

        if (brickCount >= targetLadderCount)
        {
            SwitchState(NPCState.MoveLadderZone);
            return;
        }

        SwitchState(NPCState.SearchBrick);
    }

    private void DownLadderSetting(bool isOn)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = myLadderZone[myLadderCount].transform.position + new Vector3(0, 0, -1.3f);

        npcColider.isTrigger = isOn;
        npcRB.useGravity = !isOn;
    }

    private void ResetTarget()
    {
        targetTr = null;
        targetPos = Vector3.zero;
    }


    public IEnumerator stopMove1sec()
    {
        float nowSpeed = speed;

        speed = 0;
        yield return new WaitForSeconds(2f);
        speed = nowSpeed;
    }

    private void GetMoreBrick()
    {
        SwitchState(NPCState.SearchBrick);
    }
}
