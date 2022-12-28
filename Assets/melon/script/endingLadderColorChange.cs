using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingLadderColorChange : MonoBehaviour
{
    [SerializeField] Material[] color;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = color[Random.Range(0, 4)];
    }
}
