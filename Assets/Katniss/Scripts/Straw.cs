using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straw : MonoBehaviour
{
    private int fillHash;
    private float remainingCoffee = 1f;
    private float strawFill;

    [SerializeField] private Renderer strawRenderer;

    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");

        StartCoroutine(Fill());
    }

    IEnumerator Fill()
    {
        var effectTime = 4f;

        for (var time = 0f; time <= effectTime; time += Time.deltaTime)
        {
            strawFill = Mathf.Clamp(Mathf.Lerp(0f, remainingCoffee, time / effectTime), -1f, 1f);
            strawRenderer.material.SetFloat(fillHash, strawFill);

            yield return null;
        }
    }
}
