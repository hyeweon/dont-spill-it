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
        player.fillLiquidEvent += new PlayerEventHandler(fillCoffee);
        player.rotStartEvent += new PlayerEventHandler(StartRotRoad);
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
    }

    void fillCoffee()
    {
        fillMachine.Play();
        coffee.FillLiquid_Full();

        StartCoroutine(FinishFillCoffee());
    }

    void StartRotRoad()
    {
        roadAnimator.enabled = true;
    }

    void ShowEnding()
    {
        straw.changeFill(coffee.coffeeFill);
        blocks.changeBlock(coffee.coffeeFill);
    }

    IEnumerator FinishFillCoffee()
    {
        yield return new WaitForSeconds(0.2f);

        fillMachine.Stop();
    }
}
