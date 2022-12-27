using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private float effectBlockCount;

    [SerializeField] private Renderer[] blockRenderers;

    void Start()
    {
        
    }

    IEnumerator BlockEffect()
    {
        var effectTime = 3f;
        var cycleTime = effectTime / effectBlockCount;
        var idx = 0;

        for (var time = 0f; time <= effectTime; time += Time.deltaTime)
        {
            if (Mathf.Lerp(0f, effectBlockCount, time / effectTime) < cycleTime * idx)
                yield break;

            idx++;

            //blockRenderers[idx].material;

            yield return null;
        }


    }
}
