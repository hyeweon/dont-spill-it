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

    //void Start()
    //{
    //    agent.SetDestination(goal.transform.position);
    //    agent.updateRotation = false;

    //}

    private void OnEnable()
    {
        playerAnimator.enabled = true;
        agent.SetDestination(goal.transform.position);
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstTouch.x = Input.mousePosition.x;
            isMove = true;
            // 이전과 지금 중에 지금이 0에 더 가까우면 올라가는중
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            lastTouch.x = Input.mousePosition.x;
            dir = lastTouch - firstTouch;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMove = false;
        }
    }

    private void LateUpdate()
    {
        //Move();
            Rotate();
        if (isActiveRot && !isMove)
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

    public void ChangeRot(bool _isRight)
    {
        isActiveRot = true;
        isRight = _isRight;
    }

    public void inActiveAutoRot()
    {
        isActiveRot = false;
        isRight = false;
    }

    private void AutoRotate()
    {
        if (isRight == true && autoRotMultiply < 0)
        {
            autoRotMultiply *= -1;
        }
        if (isRight == false && autoRotMultiply > 0)
        {
            autoRotMultiply *= -1;
        }

        autoRotSpeed += Time.deltaTime * autoRotMultiply;

        var angle = lastRot.z - (autoRotSpeed);
        //angle = (angle > 180) ? angle % 360 : angle;
        angle = Mathf.Clamp(angle, -180, 180);


        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.Rotate(angle * Vector3.forward * Time.deltaTime);
        lastRot = spineTr.rotation;
    }

    private void Rotate()
    {
        var angle = transform.eulerAngles.z + -dir.x;
        angle = Mathf.Clamp(angle, -rotClamp, rotClamp);

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.rotation = Quaternion.Slerp(lastRot, rot, rotSpeed * Time.deltaTime);
        lastRot = spineTr.rotation;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == 9) // left
    //    {
    //        isActiveRot = true;
    //        isRight = false;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == 9) // left
    //    {
    //        isActiveRot = false;
    //    }
    //}
}
