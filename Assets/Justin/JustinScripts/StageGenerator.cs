using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] LadderGenerator generator;

    private int playerLayer;
    private int npcLayer;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("player");
        npcLayer = LayerMask.NameToLayer("NPC");
    }

    bool isActive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isActive == false && (playerLayer == other.gameObject.layer || npcLayer == other.gameObject.layer))
        {
            isActive = true;
            generator.enabled = true;
        }
    }
}
