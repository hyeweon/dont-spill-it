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

    private int autoRotRightLayer;
    private int autoRotLeftLayer;


    public event PlayerEventHandler reachGoalEvent;
    public event PlayerEventHandler fillLiquidEvent;
    public event PlayerEventHandler rotStartEvent;
    public event PlayerEventHandler complimentEvent;
    public event PlayerEventHandler autoRot_REvent;
    public event PlayerEventHandler autoRot_LEvent;
    public event PlayerEventHandler autoRot_ExitEvent;


    void Start()
    {
        goalLayer = LayerMask.NameToLayer("Goal");
        fillLayer = LayerMask.NameToLayer("Fill");
        rotStartLayer = LayerMask.NameToLayer("RotStart");
        complimentLayer = LayerMask.NameToLayer("Compliment");

        autoRotRightLayer = LayerMask.NameToLayer("AutoRotate_R");
        autoRotLeftLayer = LayerMask.NameToLayer("AutoRotate_L");
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

        if (other.gameObject.layer == autoRotRightLayer)
        {
            print("right");
            autoRot_REvent();
        }
        else if (other.gameObject.layer == autoRotLeftLayer)
        {
            print("left");
            autoRot_LEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == autoRotRightLayer || other.gameObject.layer == autoRotLeftLayer)
        {
            print("exit");
            autoRot_ExitEvent();
        }
    }
}
