using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Straw : MonoBehaviour
{
    private int fillHash;
    private float remainingCoffee;
    private float strawFill;

    private float startPos = 30f;
    private float targetPos = 17.5f;

    [SerializeField] private Renderer strawRenderer;

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

        if (remainingCoffee > 0.9f)
        {
            //fanfare
            //effect
        }
    }
}
