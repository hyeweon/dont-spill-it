using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldReset : MonoBehaviour
{
    Blocks blocks;

    private void Reset()
    {
        blocks = GameObject.Find("Blocks").GetComponent<Blocks>();

        for(int i = 0; i < 40; i++)
        {
            blocks.blockRenderers[i] = GameObject.Find($"block ({i})").GetComponent<Renderer>();
        }
    }
}
