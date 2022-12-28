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
    [SerializeField] Animator roadAnimator;

    void Start()
    {
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
        player.fillLiquidEvent += new PlayerEventHandler(fillCoffee);
        player.rotStartEvent += new PlayerEventHandler(StartRotRoad);
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

    void StartRotRoad()
    {
        roadAnimator.enabled = true;
    }
}
