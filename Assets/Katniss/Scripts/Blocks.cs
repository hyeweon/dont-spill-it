using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private int effectBlockCount;

    [SerializeField] private Color[] colors;
    [SerializeField] FollowPlayer followPlayer;

    public Renderer[] blockRenderers;

    public void changeBlock(float _remainingCoffee)
    {
        var _effectBlockCount = (_remainingCoffee - 0.5f) * 80f;
        effectBlockCount = (int)_effectBlockCount;
        Debug.Log(effectBlockCount);
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
                followPlayer.ChangeTarget(blockRenderers[idx].transform);

                blockRenderers[idx].material.color = colors[idx / 4];
                idx++;
            }

            yield return null;
        }


    }
}
