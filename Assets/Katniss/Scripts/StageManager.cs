using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] Image fadeOutPanel;

    void Start()
    {
        player.fillLiquidEvent += new PlayerEventHandler(fillCoffee);
        player.rotStartEvent += new PlayerEventHandler(StartRotRoad);
        player.reachGoalEvent += new PlayerEventHandler(ShowEnding);
        player.complimentEvent += new PlayerEventHandler(Compliment);

        player.autoRot_REvent += new PlayerEventHandler(RotateRight);
        player.autoRot_LEvent += new PlayerEventHandler(RotateLeft);
        player.autoRot_ExitEvent += new PlayerEventHandler(RotateExit);

        coffee.gameOverEvent += new CoffeeEventHandler(GameOver);
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
        roadAnimator.SetBool("SetRight", true);
    }

    void ShowEnding()
    {
        StartCoroutine(FinishEnding());
    }

    void GameOver()
    {
        Time.timeScale = 0f;

        playerMove.Stop();
        StartCoroutine(FadeOut());
    }

    IEnumerator FinishEnding()
    {
        coffee.MoveToFinalPos();
        straw.MoveDown();
        playerMove.enabled = false;

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

    IEnumerator FadeOut()
    {
        var fadeOutTime = 0.5f;
        var color = Color.black;

        for (var time = 0f; time < fadeOutTime; time += Time.unscaledDeltaTime)
        {
            color.a = time / fadeOutTime;
            fadeOutPanel.color = color;
            yield return null;
        }

        fadeOutPanel.color = Color.black;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
