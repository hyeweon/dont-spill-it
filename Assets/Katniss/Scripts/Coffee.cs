using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    private float initialAngleThreshold = 0.1f;
    private float finalAngleThreshold = 45f;
    private float currAngleThreshold;
    private float timeThreshold = 0.3f;
    private float timeCount = 0f;
    private float coffeeFill;

    [SerializeField] private Renderer coffeeRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currAngleThreshold = initialAngleThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.rotation.z < currAngleThreshold && gameObject.transform.rotation.z > -1 * currAngleThreshold)
            return;

        timeCount += Time.deltaTime;
        if (timeCount > timeThreshold)
        {
            coffeeFill = Mathf.Clamp(coffeeRenderer.material.GetFloat("_Fill") - 0.1f, -1f, 1f);
            coffeeRenderer.material.SetFloat("_Fill", coffeeFill);

            if (coffeeFill < 0.5f)
            {
                Debug.Log("Game Over");
            }

            timeCount = 0;
            Debug.Log("spill");
        }
    }
}
