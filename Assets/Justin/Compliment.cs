using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Compliment : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] float size = 0;

    bool isDo = false;

    public void ComplimentOn()
    {
        StartCoroutine(TextOn());
    }

    private IEnumerator TextOn()
    {
        tmp.DOFade(1, 0);
        if (isDo == false)
        {
            isDo = true;
            float timer = 0;
            while (0.5f > timer)
            {
                yield return null;

                timer += Time.deltaTime;
                tmp.fontSize += size * Time.deltaTime;
            }

            timer = 0;
            while (0.5f > timer)
            {
                yield return null;

                timer += Time.deltaTime;
                tmp.fontSize -= size * Time.deltaTime;
            }
            tmp.fontSize = 0;
            tmp.DOFade(0, 0.3f);
        }
    }
}
