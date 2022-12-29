using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private Vector3 finalPos = new Vector3(-1.67999995f, 1.20000005f, 313.950012f);
    [SerializeField] private Vector3 finalPos2 = new Vector3(-1.67999995f, 3.31f, 313.950012f);

    [SerializeField] private Renderer coffeeRenderer;
    [SerializeField] private ParticleSystem[] coffeeParticleSystem;

    [SerializeField] private Warning warning;
    Sequence sequence;

    void Start()
    {
        fillHash = Shader.PropertyToID("_Fill");

        currAngleThreshold = initialAngleThreshold;

        coffeeFill = 1f;
        coffeeRenderer.material.SetFloat(fillHash, coffeeFill);

        sequence = DOTween.Sequence().SetAutoKill(false).Pause();
        sequence.Append(transform.DOMove(finalPos2, 1f));
        sequence.Join(transform.DORotate(new Vector3(-5,0,0), 0.5f));
        sequence.Append(transform.DOMove(finalPos, 1f));
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

            warning.WarningTextOn();
            if (transform.rotation.z < 0)
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

    public void MoveToFinalPos()
    {
        transform.parent = null;
        sequence.Restart();
    }

    public void FillLiquid_Full()
    {
        StartCoroutine(FillByCoroutine(3));
    }

    IEnumerator FillByCoroutine(float _duration)
    {
        float time = 0;
        float value = coffeeRenderer.material.GetFloat(fillHash);

        while (time < _duration)
        {
            time += Time.deltaTime;
            value = Mathf.Lerp(value, 1, time / _duration);
            coffeeRenderer.material.SetFloat(fillHash, value);

            yield return null;
        }

        coffeeRenderer.material.SetFloat(fillHash, 1);
    }
}
