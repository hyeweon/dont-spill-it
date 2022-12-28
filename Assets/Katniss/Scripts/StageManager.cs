using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Coffee coffee;
    [SerializeField] Straw straw;
    [SerializeField] Blocks blocks;
    [SerializeField] ParticleSystem fillMachine;

    void Start()
    {
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
        player.fillLiquidEvent += new PlayerEventHandler(fillCoffee);
    }

    void ShowEnding()
    {
        straw.changeFill(coffee.coffeeFill);
        blocks.changeBlock(coffee.coffeeFill);
    }

    void fillCoffee()
    {
        fillMachine.Play();
        coffee.FillLiquid_Full();
    }
}
