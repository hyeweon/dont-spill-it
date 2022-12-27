using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Coffee coffee;
    [SerializeField] Straw straw;
    [SerializeField] Blocks blocks;

    void Start()
    {
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
    }

    void ShowEnding()
    {
        straw.changeFill(coffee.coffeeFill);
        blocks.changeBlock(coffee.coffeeFill);
    }
}
