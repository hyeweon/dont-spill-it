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
    private float coffeeFill;
    private float gameOverFill = 0.5f;

    [SerializeField] private Renderer coffeeRenderer;
    [SerializeField] private ParticleSystem coffeeParticleSystem;

    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");

        currAngleThreshold = initialAngleThreshold;
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
            coffeeParticleSystem.Play();

            if (coffeeFill < gameOverFill)
            {
                Debug.Log("Game Over");
            }

            timeCount = 0;
            currAngleThreshold = Mathf.Lerp(initialAngleThreshold, finalAngleThreshold, (1 - coffeeFill) / gameOverFill);

            Debug.Log($"spill {currAngleThreshold}");
        }
    }
}
