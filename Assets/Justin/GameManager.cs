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
        print("ㅎㅏ이루000");

        startCount.OnCount((result) => {
            print("ㅎㅏ이루111");

            if (result == true)
            {
                stageManager.enabled = true;
                playerMove.enabled = true;
            }
        });

        print("ㅎㅏ이루222");
    }

}
