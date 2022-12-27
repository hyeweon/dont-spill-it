using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerEventHandler();

public class Player : MonoBehaviour
{
    private int goalLayer;

    public event PlayerEventHandler reachGoalEvent;

    void Start()
    {
        goalLayer = LayerMask.NameToLayer("Goal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == goalLayer)
        {
            reachGoalEvent();
        }
    }
}
