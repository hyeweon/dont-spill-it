using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class StartCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_Num;

    public void OnCount(Action<bool> _callback = null)
    {
        StartCoroutine(Count(3, _callback));
    }

    IEnumerator Count(int _targetCount, Action<bool> _callback = null)
    {
        tmp_Num.DOFade(1, 0);
        float num = _targetCount;
        while(num >= 0)
        {
            yield return null;
            num -= Time.deltaTime;
            tmp_Num.text = $"{Mathf.RoundToInt(num)}";
        }

        tmp_Num.text = $"{0}";
        tmp_Num.DOFade(0, 0.5f);
        this.gameObject.SetActive(false);
        _callback(true);
    }
}
