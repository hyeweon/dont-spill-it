using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] Coffee coffee;
    [SerializeField] Straw straw;
    [SerializeField] Blocks blocks;
    [SerializeField] Compliment compliment;
    [SerializeField] ParticleSystem fillMachine;
    [SerializeField] Animator roadAnimator;

    void Start()
    {
        player.fillLiquidEvent += new PlayerEventHandler(fillCoffee);
        player.rotStartEvent += new PlayerEventHandler(StartRotRoad);
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
        player.complimentEvent += new PlayerEventHandler(Compliment);

        player.autoRot_REvent += new PlayerEventHandler(RotateRight);
        player.autoRot_LEvent += new PlayerEventHandler(RotateLeft);
        player.autoRot_ExitEvent += new PlayerEventHandler(RotateExit);
    }

    void fillCoffee()
    {
        //fillMachine.Play();
        coffee.FillLiquid_Full();

        StartCoroutine(FinishFillCoffee());
    }

    void StartRotRoad()
    {
        roadAnimator.enabled = true;
    }

    void ShowEnding()
    {
        StartCoroutine(FinishEnding());
    }

    IEnumerator FinishEnding()
    {
        coffee.MoveToFinalPos();
        straw.MoveDown();

        yield return new WaitForSeconds(1.3f);

        straw.changeFill(coffee.coffeeFill);
        blocks.changeBlock(coffee.coffeeFill);
    }

    void Compliment()
    {
        compliment.ComplimentOn();
    }

    IEnumerator FinishFillCoffee()
    {
        yield return new WaitForSeconds(0.1f);

        fillMachine.Stop();
    }

    void RotateRight()
    {
        playerMove.ChangeRot(true);
    }

    void RotateLeft()
    {
        playerMove.ChangeRot(false);
    }

    void RotateExit()
    {
        playerMove.inActiveAutoRot();
    }
}
