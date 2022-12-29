using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    [SerializeField] private PlayerMove playerMove;

    [SerializeField] private StartCount startCount;

    private void Awake()
    {
        stageManager.enabled = false;
        playerMove.enabled = false;

        startCount.OnCount((result) => {
            if (result == true)
            {
                stageManager.enabled = true;
                playerMove.enabled = true;
            }
        });
    }

}
