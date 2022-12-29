using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Warning : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private string[] careful_contents;
    [SerializeField] private float size = 55;
    [SerializeField] private bool isDo = false;


    public void WarningTextOn()
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

            tmp.text = careful_contents[Random.Range(0, careful_contents.Length)];

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
            tmp.DOFade(0, 0.3f);
            isDo = false;
        }
    }

}
