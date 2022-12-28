using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ladderStack : MonoBehaviour
{
    [SerializeReference] protected List<GameObject> slot = new List<GameObject>();
    [SerializeField] private int hangLayer => LayerMask.NameToLayer("Hang");



    /// <summary> 비트 레이어(레이 검출용) </summary>
    protected int targetLayer_ray;
    /// <summary> int 레이어(충돌 검출용) </summary>
    [SerializeField] protected int targetLayer_collision;

    [SerializeField] protected Transform backpos;
    //캐릭터의 등뒤에 쌓을곳의 좌표 localPostion으로 할당할것.
    public Vector3 setBackPos()
    {
        //피벗정상화시 활성화해야하는 부분
        //return new Vector3(0, 0.5f, -0.4f);
        return new Vector3(0, 0.2f, -0.07f);

        //피벗 맞추기위한 임시부분
        //return new Vector3(0.24f, 0.2f, -0.02f);
    }

    /* 획득한 사다리를 등뒤에 쌓는 함수
     * backPostion  - 플레이어 기준 등에 존재하는 빈오브젝트
     * ladder       - 충돌할 오브젝트
     * parent       - 오브젝트를 획득한 캐릭터, 획득오브젝트 자식화
     * slot         - 획득한 오브젝트 관리 list              */
    Ease ease;
    float yPos = 0.2f;
    public void stackBack(GameObject ladder, Transform parent)
    {
        //획득한 사다리를 캐릭터 등뒤 backPosition으로 옮긴다
        //ladder.transform.position = backPos.position;
        // ladder.layer = hangLayer;

        var pos = backpos.localPosition;
        ladder.transform.DOLocalMove(pos, 0.3f);
        ladder.transform.SetParent(parent);

        ladder.transform.localRotation = Quaternion.Euler(0, 0, 0);
        ladder.GetComponent<Collider>().enabled = false;

        //사다리를 획득했다면 다음 쌓을 사다리의 위치를 조정한다.
        //backPosition.Translate(0, 0.2f, 0);
        //backPos.Translate(0, 0.2f, -0.07f);
        backpos.localPosition += new Vector3(0, 0.15f, -0.07f);


        //획득한 사다리를 slot에 등록한다.
        slot.Add(ladder);

        //backpos = backPos;
    }


    //현재 획득한 사다리의 개수 확인
    public int CountLadder()
    {
        return slot.Count;
    }

    //획득한 사다리를 처음 좌표로 돌려보내고 자신의 리스트에서 제거
    public void deleteSlot()
    {
        if (slot.Count - 1 < 0)
            return;

        slot[slot.Count - 1].GetComponent<LadderBack>().backToOriginAction?.Invoke();
        slot.RemoveAt(slot.Count - 1);

        backpos.localPosition = setBackPos();
    }



    //쌓은 사다리 설치 함수 (현재 미사용, 플레이어기준 설치시 사용할것)
    public IEnumerator putLadder(Transform putPos, Transform backPosition)
    {
        //획득한 사다리가 없다면 종료
        if (slot.Count == 0)
            yield break;

        //일정시간마다 사다리를 putPos위치에 설치
        var waitTime = new WaitForSeconds(0.1f);


        while (slot.Count != 0)
        {
            //슬롯이 빌때까지 사다리를 설치한다.
            slot[slot.Count - 1].transform.parent = null;
            slot[slot.Count - 1].transform.rotation = Quaternion.Euler(0, -90, -90);
            slot[slot.Count - 1].transform.position = putPos.position;
            //if(slot.Count > 1)
            //    downBackPos();
            slot.RemoveAt(slot.Count - 1);

            //사다리가 설치될때마다 다음 사다리를 놓을 putPos의 좌표, 등뒤에 쌓을 backPos의 좌표를 변경해준.
            putPos.Translate(0, -0.8f, 0);
            //backPosition.Translate(0, -0.2f, 0);
            yield return waitTime;
        }


    }


    //다음으로 쌓을 사다리의 위치를 내리는 함수
    //public void downBackPos()
    //{
    //    Debug.Log("down");
    //    //backpos.Translate(0, -0.2f, 0);

    //    backpos.localPosition = new Vector3(0, -0.15f, 0.07f);
    //    //return new Vector3(0, 0.2f, -0.07f);
    //}

    //기믹(바나나)에 충돌했을때 사용할 함수
    public void GimmickBanana()
    {
        if (slot.Count == 0)
            return;
        Rigidbody ladderRig;
        Vector3 explosionPos = slot[(slot.Count - 1) / 2].transform.position;

        for (int i = 0; i < slot.Count; i++)
        {
            slot[i].transform.parent = null;
            slot[i].layer = targetLayer_collision;
            slot[i].GetComponent<Collider>().enabled = true;

            ladderRig = slot[i].GetComponent<Rigidbody>();
            ladderRig.constraints = RigidbodyConstraints.None;
            ladderRig.AddExplosionForce(300, explosionPos, 5);

            Destroy(slot[i].gameObject, 2);

            //downBackPos();
        }

        if (this.gameObject.layer >= 14)
        {
            this.GetComponent<NonPlayer>().GetMoreAction?.Invoke();
        }
        backpos.localPosition = setBackPos();
        clearSlot();
    }


    //스택한 리스트를 비워주는 함수
    public void clearSlot()
    {
        for (int i = 0; i < slot.Count; i++)
        {
            slot[i].transform.parent = null;

        }
        while (slot.Count > 0)
        {
            slot.RemoveAt(slot.Count - 1);
        }
    }

    public void ClearChild()
    {
        for (int i = 0; i < slot.Count; i++)
        {
            slot[i].transform.parent = null;
            slot[i].gameObject.SetActive(false);
        }
    }
}
