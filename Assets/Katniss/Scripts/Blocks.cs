using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private int effectBlockCount;

    [SerializeField] private Color[] colors;

    public Renderer[] blockRenderers;

    void Start()
    {
        effectBlockCount = 40;
        StartCoroutine(BlockEffect());
    }

    IEnumerator BlockEffect()
    {
        var effectTime = 4f;
        int idx = 0;

        for (var time = 0f; time < effectTime; time += Time.deltaTime)
        {
            if (Mathf.Lerp(0f, (float)effectBlockCount, time / effectTime) > idx)
            {
                blockRenderers[idx].material.color = colors[idx / 4];
                idx++;
            }

            yield return null;
        }


    }
}
