using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Transform camTr;
    [SerializeField] private Transform[] camEndingTargetTr;
    private int endingCount = 0;

    [Header("Player")]
    [SerializeField] private Transform playerTr;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private playerMove playerMove;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem ps_Trail;
    [SerializeField] private ParticleSystem[] confettis;
    [SerializeField] private ParticleSystem[] finals;
    [SerializeField] private CanvasGroup endUI;

    [SerializeField] private FollowCamera followCamera;
    [SerializeField] private Transform goal;

    private void OnEnable()
    {
        camTr.position = new Vector3(53.7920685f, -13.7700005f, -38.2599983f);
        endingCount++;
        camTr.rotation = Quaternion.AngleAxis(90, Vector3.up);
        followCamera.enabled = false;
        playerMove.enabled = false;
        StartCoroutine(startGoingFront());
    }


    IEnumerator startGoingFront()
    {
        playerAnimator.SetBool("RunCheck", true);
        yield return new WaitForSeconds(0.3f);
        playerAnimator.SetBool("RunCheck", false);
        playerAnimator.SetBool("Lay", true);

        camTr.parent = playerTr;
        camTr.DOLocalMove(new Vector3(0, 0.907f, -0.831f), 1f);
        //Vector3(77.1722641, 90.0000381, 2.30728256e-05)
        //camTr.DOLocalRotate(new Vector3(transform.position.x + 5, 0, 0), 0.5f);
        camTr.DORotate(new Vector3(77.1722641f, 90, 2.3f), 1f);
        foreach (var confetti in confettis)
            confetti.Play();

        playerTr.DORotate(new Vector3(37.679f, 90, 0), 0.5f);
        playerTr.DOMoveX(64, 1);
        ps_Trail.Play();
        yield return new WaitForSeconds(0.5f);

        playerRb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(6.5f);

        ps_Trail.Stop();
        StartCoroutine(secondEndingDance());
    }

    IEnumerator secondEndingDance()
    {
        yield return null;
        playerAnimator.SetBool("Victory", true);
        playerAnimator.SetBool("Lay", false);

        camTr.parent = null;
        playerTr.DORotate(new Vector3(0, 90, 0), 0);
        playerTr.GetComponent<Rigidbody>().isKinematic = true;

        camTr.DOMove(camEndingTargetTr[endingCount].position, 0.5f);// .position = camEndingTargetTr[endingCount].position;
        camTr.DORotate(new Vector3(0, -90f, 0), 1);
        foreach (var item in finals)
            item.Play();
        yield return new WaitForSeconds(5f);
        foreach (var item in finals)
            item.Play();
        playerAnimator.SetBool("Dance", true);
        playerAnimator.SetBool("Victory", false);
        foreach(var item in finals)
        {
            item.Play();
        }
        camTr.DOMoveX(111.51f, 10);
        yield return new WaitForSeconds(3f);

        
        endUI.DOFade(1, 0.5f);
    }

}
