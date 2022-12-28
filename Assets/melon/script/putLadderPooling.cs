using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class putLadderPooling : MonoBehaviour
{
    [SerializeField] private int ladderCount;
    [SerializeField] private GameObject ladder;
    [SerializeField] private List<GameObject> ladderSlot;
    [SerializeField] private playerMove player;
    [SerializeField] private GameObject downTrigger;
    [SerializeField] private FollowCamera follwcamera;
    [SerializeField] private GameObject idleZone;
    [SerializeField] private bool horizontalCheck;
    [SerializeField] private bool verticalCheck;
    [SerializeField] private bool endingCheck;

    public Action<ladderStack, Action<bool>> setLadderAction { get; private set; }
    [SerializeField] private int npcLayer;

    Vector3 putPos;
    Vector3 NextPos;
    Quaternion nextRot;
    GameObject obj;
    LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        setNeedladderCount();
        poolingLadder();

        playerLayer = 1 << LayerMask.NameToLayer("player");
        npcLayer = LayerMask.NameToLayer("NPC");
        setLadderAction = StartOnLadder;
    }

    void setNeedladderCount()
    {
        if (verticalCheck)
        {
            // 휴먼 사다리에 맞춰서 값 수정
            ladderCount = Mathf.CeilToInt((transform.position.y - idleZone.transform.position.y) / 2) - 1;
        }
        else if (horizontalCheck)
        {
            //ladderCount = Mathf.CeilToInt(Mathf.Abs(transform.position.x - idleZone.transform.position.x));
            ladderCount = Mathf.CeilToInt(Mathf.Abs((transform.position.x - idleZone.transform.position.x) / 2) - 1);
        }
        else if (endingCheck)
        {
            ladderCount = Mathf.CeilToInt(Mathf.Abs(Vector3.Distance(transform.position, idleZone.transform.position)));
        }
    }


    //putZone오브젝트를 기준으로 벽에 일정량의 사다리를 풀링생성하는 함수
    void poolingLadder()
    {
        //첫사다리가 설치될 좌표를 putZone기준으로 정한뒤 아래로 내려갈 좌표값을 설정
        if (verticalCheck)
        {
            // 휴먼 사다리에 맞춰서 값 수정
            putPos = transform.position + new Vector3(0, -2, -0.7f);

            //NextPos = new Vector3(0, -0.8f, 0); // 기존
            NextPos = new Vector3(0, -2f, 0);
            //nextRot = Quaternion.Euler(0, -90, -90); // 기존
            nextRot = Quaternion.Euler(0, 0, 0); // 변겨
        }
        else if (horizontalCheck)
        {
            putPos = transform.position + new Vector3(2f, 0, 0);
            NextPos = new Vector3(2f, 0, 0);
            nextRot = Quaternion.Euler(90, 0, 90);
            //putPos = transform.position + new Vector3(0.8f, 0, 0);
            //NextPos = new Vector3(0.8f,0, 0);
            //nextRot = Quaternion.Euler(0, 0, 0);
        }
        else if (endingCheck)
        {
            //마지막 시작 발판과 엔딩발판의 거리차를 이용해 생성될 계단의 각도와 높이를 수정한다.
            float DisX = transform.position.x - idleZone.transform.position.z;
            float DisY = transform.position.y - idleZone.transform.position.y;
            float DisZ = transform.position.z - idleZone.transform.position.z;
            float angle = Mathf.Atan2(DisY, DisZ) * Mathf.Rad2Deg;
            float Yheight = angle * Mathf.Deg2Rad;

            putPos = transform.position + new Vector3(0, 0, -0.8f);
            NextPos = new Vector3(0, -Yheight, -0.8f);
            nextRot = Quaternion.Euler(0, 90, -angle);
        }

        //일정개수의 사다리를 풀링으로 사전생성
        for (int i = 0; i < ladderCount; i++)
        {
            obj = Instantiate(ladder, putPos, nextRot);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            putPos += NextPos;

            //사다리관리를 위한 list에 할당
            ladderSlot.Add(obj);
        }
    }

    //플레이어가 putZone에 도달했을시 사다리설치 코루틴 활성화
    private void OnTriggerEnter(Collider other)
    {
        if (13 == other.gameObject.layer)
        {
            StartCoroutine(onLadder(other.GetComponent<ladderStack>()));
            return;
        }
    }

    private void StartOnLadder(ladderStack stack, Action<bool> doneCallback = null)
    {
        StartCoroutine(onLadder(stack, doneCallback));
    }

    //플레이어가 putZone에 도달했을떄 사다리를 설치하기위한 코루틴
    private IEnumerator onLadder(ladderStack stack, Action<bool> doneCallback = null)
    {
        //사다리를 가지고있지 않다면 작업하지 않는다.
        if (stack.CountLadder() == 0)
            yield break;


        //딜레이 시간은 0.1초
        var time = new WaitForSeconds(0.2f);

        print($"name: {stack.gameObject.name} count: {stack.CountLadder()}");
        print($"사다리 개수: {CountOnladder()}");

        var maxladderCount = CountOnladder() + stack.CountLadder();
        //활성화된 사다리의 개수를 확인한뒤 플레이어가 가진 사다리의 개수만큼 풀링된 사다리를 활성화한다
        for (int i = CountOnladder(); i < maxladderCount; i++)
        {
            print("몇번 동작?");
            if (i < ladderSlot.Count && !ladderSlot[i].activeSelf)
            {
                ladderSlot[i].SetActive(true);

                //사다리가 활성화 될때마다 카메라가 활성화된 사다리를 따라간다.
                if (stack.gameObject.layer == 13)
                    follwcamera.changeTarget(ladderSlot[i].transform);

                //stack.downBackPos(); //사다리 설치시 backpos위치 조정
                stack.deleteSlot();
                yield return time;
            }
        }

        // 사다리가 가득차면 사다리 아래로 이동
        if (stack.gameObject.layer == npcLayer && CountOnladder() >= ladderCount)
        {
            downTrigger.transform.parent = null;
            this.GetComponent<Collider>().enabled = false;

            if (verticalCheck)
            {
                doneCallback?.Invoke(true);
            }
            else if (horizontalCheck)
            {
                doneCallback?.Invoke(false);
            }

            this.enabled = false;
        }
        else if(stack.gameObject.layer == npcLayer)
        {
            // 부족하면 더 구해오기
            stack.GetComponent<NonPlayer>().GetMoreAction?.Invoke();
        }

        // 플레이어일 때만 동작
        if (stack.gameObject.layer == 13)
            follwcamera.targetToPlayer();

        if (ladderSlot[ladderCount - 1].activeSelf)
        {
            downTrigger.SetActive(true);
        }
    }


    //벽에 설치된 사다리가 몇개가 켜져있는지 확인
    int CountOnladder()
    {
        int num = 0;
        for (int i = 0; i < ladderSlot.Count; i++)
        {
            if (ladderSlot[i].activeSelf)
            {
                num++;
            }
        }

        return num;
    }

    public GameObject returnGoalObj()
    {
        return idleZone;
    }
}
