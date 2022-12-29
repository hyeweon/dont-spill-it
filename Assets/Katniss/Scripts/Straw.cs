using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Straw : MonoBehaviour
{
    private int fillHash;
    private float remainingCoffee;
    private float strawFill;

    private float targetPos = 77f;

    [SerializeField] private ParticleSystem ps_JuiceEffect;
    [SerializeField] private Renderer strawRenderer;
    [SerializeField] private CanvasGroup cg_EndUI;
    [SerializeField] private ParticleSystem fanfarePS;
 
    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");

        strawFill = 0f;
        strawRenderer.material.SetFloat(fillHash, strawFill);
    }

    public void changeFill(float _remainingCoffee)
    {
        remainingCoffee = _remainingCoffee;
        StartCoroutine(Fill());
    }

    public void MoveDown()
    {
        transform.DOMoveY(targetPos, 1.3f);
    }

    IEnumerator Fill()
    {
        var effectTime = 3f;

        for (var time = 0f; time <= effectTime; time += Time.deltaTime)
        {
            strawFill = Mathf.Clamp(Mathf.Lerp(0f, remainingCoffee, time / effectTime), 0f, 1f);
            strawRenderer.material.SetFloat(fillHash, strawFill);

            yield return null;
        }

        strawFill = Mathf.Clamp(remainingCoffee, 0f, 1f);
        strawRenderer.material.SetFloat(fillHash, strawFill);

        if (remainingCoffee > 0.9f)
        {
            //fanfare
            //effect
            ps_JuiceEffect.Play();
            yield return new WaitForSeconds(2);
        }

        cg_EndUI.DOFade(1, 0.6f);

        fanfarePS.Play();
    }
}
