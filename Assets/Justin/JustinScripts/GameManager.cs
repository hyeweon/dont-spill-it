using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private playerMove playerMove;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private NonPlayer[] nonPlayers;
    [SerializeField] private LadderGenerator firstGenerator;

    [SerializeField] private CanvasGroup startUI;
    [SerializeField] private CanvasGroup InGameUI;

    private void Awake()
    {
        firstGenerator.enabled = true;
        Time.timeScale = 0;
    }

    void Update()
    {
        if (isStarted == false && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(StarGame());
        }

    }

    bool isStarted = false;
    private IEnumerator StarGame()
    {
        Time.timeScale = 1;
        playerRB.useGravity = true;
        isStarted = true;
        startUI.DOFade(0, 0.5f);
        foreach (var item in nonPlayers)
            item.enabled = true;
        yield return new WaitForSeconds(0.3f);
        InGameUI.DOFade(1, 0);

        playerMove.enabled = true;
        
    }
}
