using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    private int fillHash;
    private float initialAngleThreshold = 0.1f;
    private float finalAngleThreshold = 0.4f;
    private float currAngleThreshold;
    private float timeThreshold = 0.1f;
    private float timeCount = 0f;
    private float gameOverFill = 0.5f;

    public float coffeeFill;

    [SerializeField] private Renderer coffeeRenderer;
    [SerializeField] private ParticleSystem[] coffeeParticleSystem;

    [SerializeField] private Player player;

    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");

        currAngleThreshold = initialAngleThreshold;

        coffeeFill = 1f;
        coffeeRenderer.material.SetFloat(fillHash, coffeeFill);

        player.fillLiquidEvent += new PlayerEventHandler(FillLiquid_Full);
    }

    void Update()
    {
        if (gameObject.transform.rotation.z < currAngleThreshold && gameObject.transform.rotation.z > -1 * currAngleThreshold)
            return;

        timeCount += Time.deltaTime;
        if (timeCount > timeThreshold)
        {
            coffeeFill = Mathf.Clamp(coffeeRenderer.material.GetFloat(fillHash) - 0.05f, -1f, 1f);
            coffeeRenderer.material.SetFloat(fillHash, coffeeFill);

            if(transform.rotation.z < 0)
                coffeeParticleSystem[0].Play();
            else
                coffeeParticleSystem[1].Play();

            if (coffeeFill < gameOverFill)
            {
                Debug.Log("Game Over");
            }

            timeCount = 0;
            currAngleThreshold = Mathf.Lerp(initialAngleThreshold, finalAngleThreshold, (1 - coffeeFill) / gameOverFill);

            //Debug.Log($"spill {currAngleThreshold}");
        }
    }

    private void FillLiquid_Full()
    {
        StartCoroutine(FillByCoroutine());
    }

    IEnumerator FillByCoroutine()
    {
        while (coffeeRenderer.material.GetFloat(fillHash) < 0.98f)
        {
            yield return null;
            var value = Mathf.Lerp(0, 1, 100 * Time.deltaTime);
            print("?????: " + value);
            coffeeRenderer.material.SetFloat(fillHash, value);
        }

        coffeeRenderer.material.SetFloat(fillHash, 1);
    }
}
