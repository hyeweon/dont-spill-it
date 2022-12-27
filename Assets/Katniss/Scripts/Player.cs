using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerEventHandler();

public class Player : MonoBehaviour
{
    private int goalLayer;
    private int fillLayer;

    public event PlayerEventHandler reachGoalEvent;
    public event PlayerEventHandler fillLiquidEvent;

    void Start()
    {
        goalLayer = LayerMask.NameToLayer("Goal");
        fillLayer = LayerMask.NameToLayer("Fill");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == goalLayer)
        {
            reachGoalEvent();
        }

        if(other.gameObject.layer == fillLayer)
        {
            print("Enter");
            fillLiquidEvent();
        }
    }
}
