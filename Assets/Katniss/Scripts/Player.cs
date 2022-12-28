using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerEventHandler();

public class Player : MonoBehaviour
{
    private int goalLayer;
    private int fillLayer;
    private int rotStartLayer;
    private int complimentLayer;

    public event PlayerEventHandler reachGoalEvent;
    public event PlayerEventHandler fillLiquidEvent;
    public event PlayerEventHandler rotStartEvent;
    public event PlayerEventHandler complimentEvent;

    void Start()
    {
        goalLayer = LayerMask.NameToLayer("Goal");
        fillLayer = LayerMask.NameToLayer("Fill");
        rotStartLayer = LayerMask.NameToLayer("RotStart");
        complimentLayer = LayerMask.NameToLayer("Compliment");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == goalLayer)
        {
            reachGoalEvent();
        }

        else if(other.gameObject.layer == fillLayer)
        {
            fillLiquidEvent();
        }

        else if (other.gameObject.layer == rotStartLayer)
        {
            rotStartEvent();
        }

        else if (other.gameObject.layer == complimentLayer)
        {
            complimentEvent();
        }
    }
}
