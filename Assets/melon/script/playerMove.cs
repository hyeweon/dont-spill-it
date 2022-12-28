using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerMove : ladderStack
{
    Vector3 clickPos;
    Vector3 dragPos;
    Vector3 direction;

    float angle;

    bool downCheck;
    bool horizonCheck;

    Animator ani;

    private int ladderBlockLayer;
    private int putZoneLayer;
    private int downTrigger;
    private int IdleTrigger;
    private int horizonTrigger;
    private int endingTrigger;
    private int gimmick;


    [SerializeField] private float speed;
    //[SerializeField] private Transform backPos;
    //[SerializeField] private Transform putPos;
    //[SerializeField] private List<GameObject> ladderSlot;

    // Start is called before the first frame update
    void Start()
    {
        // myColorLayer = 1 << targetColorLayer;
        // targetColorLayer = this.gameObject.layer;

        ani = GetComponent<Animator>();

        //ladderSlot = new List<GameObject>();

        ladderBlockLayer = LayerMask.NameToLayer("ActiveBrick");
        putZoneLayer = 1 << LayerMask.NameToLayer("putZone");
        downTrigger = LayerMask.NameToLayer("downTrigger");
        IdleTrigger = LayerMask.NameToLayer("IdleTrigger");
        horizonTrigger = LayerMask.NameToLayer("horizonTrigger");
        endingTrigger = LayerMask.NameToLayer("endingTrigger");
        gimmick = LayerMask.NameToLayer("gimmick");

        backpos.localPosition = setBackPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setClickPos();
        }

        if (Input.GetMouseButton(0))
        {
            DragMove();
        }

        if (Input.GetMouseButtonUp(0))
        {
            stopMove();
        }
    }

    void setClickPos()
    {
        //첫 클릭 스크린좌표 clickPos에 할당
        clickPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }

    void DragMove()
    {
        //클릭후 움직이는 마우스의 스크린좌표 할당
        dragPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        //dragPos와 clickPos차이를 이용해 이동 방향 결정
        if (!downCheck && !horizonCheck)
            direction = new Vector3(dragPos.x - clickPos.x, 0, dragPos.y - clickPos.y);
        else if (downCheck)
            direction = new Vector3(0, dragPos.y - clickPos.y, 0);
        else if (horizonCheck)
            direction = new Vector3(dragPos.x - clickPos.x, 0, 0);
        direction.Normalize();

        //정규화된 방향으로 speed만큼의 속도로 이동
        transform.position += direction * speed * Time.deltaTime;

        if (!downCheck && !horizonCheck)
        {
            //dragPos와 clickPos의 차이를 값으로 플레이어의 회전값 결정 및 회전
            angle = Mathf.Atan2(dragPos.x - clickPos.x, dragPos.y - clickPos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);

            //걷는 애니메이션 실행
            ani.SetBool("RunCheck", true);
        }

        if (horizonCheck)
        {
            ani.SetBool("RunCheck", true);
        }
        else if (downCheck)
        {
            ani.SetBool("climbingDown", true);
        }


    }

    void stopMove()
    {
        //걷는 애니메이션 중지, 서있는 애니메이션 실행
        ani.SetBool("RunCheck", false);

        //좌표 초기화
        clickPos = Vector3.zero;
        dragPos = Vector3.zero;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ladderBlockLayer)
        {
            stackBack(collision.gameObject, this.transform);
        }

        if (collision.gameObject.layer == gimmick)
        {
            StartCoroutine(stopMove1sec());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            stackBack(other.gameObject, this.transform);
        }

        if (other.gameObject.layer == downTrigger)
        {
            downCheck = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = other.transform.position + new Vector3(0, 0, -0.3f);
            Debug.Log("check");
            transform.GetComponent<Rigidbody>().useGravity = false;
            //ani.SetBool("climbingDown", true);
        }

        if (other.gameObject.layer == horizonTrigger)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.position = other.transform.position + new Vector3(0, 0, 0);
            transform.GetComponent<Rigidbody>().useGravity = false;
            horizonCheck = true;
        }

        if (other.gameObject.layer == IdleTrigger)
        {
            downCheck = false;
            horizonCheck = false;
            transform.GetComponent<Rigidbody>().useGravity = true;
            ani.SetBool("RunCheck", false);
            ani.SetBool("climbingDown", false);
        }

        if (other.gameObject.layer == endingTrigger)
        {
            slideToEndingField(other.GetComponent<putLadderPooling>().returnGoalObj());
        }

    }

    void slideToEndingField(GameObject goal)
    {
        transform.DOMove(goal.transform.position, 3f);
    }


    public IEnumerator stopMove1sec()
    {
        float nowSpeed = speed;

        speed = 0;
        yield return new WaitForSeconds(2f);
        speed = nowSpeed;
    }
}
