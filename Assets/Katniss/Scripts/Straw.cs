using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straw : MonoBehaviour
{
    private int fillHash;
    private float remainingCoffee;
    private float strawFill;

    [SerializeField] private Renderer strawRenderer;

    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");
    }

    public void changeFill(float _remainingCoffee)
    {
        remainingCoffee = _remainingCoffee;
        StartCoroutine(Fill());
    }

    IEnumerator Fill()
    {
        var effectTime = 4f;

        for (var time = 0f; time <= effectTime; time += Time.deltaTime)
        {
            strawFill = Mathf.Clamp(Mathf.Lerp(0f, remainingCoffee, time / effectTime), 0f, 1f);
            strawRenderer.material.SetFloat(fillHash, strawFill);

            yield return null;
        }
    }
}
