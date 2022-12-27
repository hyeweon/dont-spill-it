using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float forwardPower;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform spineTr;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject goal;

    [SerializeField] private float rotSpeed;
    [SerializeField] private float rotClamp;

    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private Quaternion lastRot = Quaternion.identity;
    [SerializeField] private Vector3 dir;
    private bool isMove = false;

    void Start()
    {
        agent.SetDestination(goal.transform.position);
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstTouch.x = Input.mousePosition.x;
            isMove = true;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            lastTouch.x = Input.mousePosition.x;
            dir = lastTouch - firstTouch;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMove = false;
            autoRotSpeed = rotAngel;
        }
    }

    private void LateUpdate()
    {
        //Move();

        //if(isMove == true)
        Rotate();
        if (isActiveRot == true)
            AutoRotate();
    }

    private void Move()
    {
        var moveVec = new Vector3(0, 0, forwardPower);
        Vector3 pos = transform.position + (moveVec * speed * 0.005f * Time.deltaTime);
        transform.position = pos;
    }

    [SerializeField] private float autoRotSpeed = 5;
    [SerializeField] private float autoRotMultiply = 0.01f;
    [SerializeField] private bool isRight = false;
    [SerializeField] private bool isActiveRot = false;

    public void ChangeRot(bool _isRight, bool _isActiveRot)
    {
        isRight = _isRight;
        isActiveRot = _isActiveRot;
    }


    private void AutoRotate()
    {
        if (isRight == true && autoRotMultiply < 0)
        {
            print("D???");
            autoRotMultiply *= -1;
        }
        if (isRight == false && autoRotMultiply > 0)
        {
            print("0?");
            autoRotMultiply *= -1;
        }

        autoRotSpeed += Time.deltaTime * autoRotMultiply;

        var angle = lastRot.z - (autoRotSpeed);
        angle = (angle > 180) ? angle % 360 : angle;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.rotation = Quaternion.Lerp(lastRot, rot, rotSpeed * Time.deltaTime);
        lastRot = spineTr.rotation;
    }

    [SerializeField] private float rotAngel;
    private void Rotate()
    {
        var angle = rotAngel +(-dir.x * Time.deltaTime);
        angle = (angle > 180) ? angle - 360 : angle;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.rotation = Quaternion.Lerp(lastRot, rot, rotSpeed * Time.deltaTime);
        lastRot = spineTr.rotation;
        rotAngel = angle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9) // left
        {
            isActiveRot = true;
            isRight = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9) // left
        {
            isActiveRot = false;
        }
    }
}
