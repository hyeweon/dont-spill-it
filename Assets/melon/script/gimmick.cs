using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gimmick : MonoBehaviour
{
    int[] layers = new int[] { 14, 15, 16, 17 };
    playerMove pm;
    NonPlayer np;
    float nowSpeed;

    Animator ani;
    private void OnCollisionEnter(Collision collision)
    {
        //플레이어가 충돌할시 플레이어의 스크립트 할당
        if (collision.gameObject.layer == 13)
        {
            pm = collision.gameObject.GetComponent<playerMove>();
            Banana(collision);
        }
        else
        {
            //IO가 충돌할시 IO스크립트 할당
            foreach (var character in layers)
            {
                if (collision.gameObject.layer == character)
                {
                    np = collision.gameObject.GetComponent<NonPlayer>();
                    Banana(collision);
                }
            }
        }
    }

    //기믹(바나나)
    void Banana(Collision collision)
    {
        ani = collision.gameObject.GetComponent<Animator>();
        ani.SetTrigger("sweep");
        if(collision.gameObject.layer == 13)
            pm.GimmickBanana();
        else
            np.GimmickBanana();

        gameObject.SetActive(false);
    }

}
