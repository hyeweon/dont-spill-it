using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] EndingManager endingManager;
    [SerializeField] endlingMakeLadder maker;
    [SerializeField] private ladderStack ladderman;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            ladderman.ClearChild();
            collision.transform.DOMoveZ(transform.position.z, 0);
            //collision.transform.position = new Vector3(17.26f, collision.transform.position.y, transform.position.z);
            // collision.transform.DORotate(new Vector3(0, 90, 0), 0);
            endingManager.enabled = true;
            maker.enabled = true;
            this.GetComponent<Collider>().enabled = false;
        }

    }
}
