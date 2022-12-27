using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float forwardPower;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform spineTr;

    [SerializeField] private float rotSpeed;
    [SerializeField] private float rotClamp;

    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private Quaternion lastRot;
    private Vector3 dir;
    private bool isMove = false;


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
        }
    }

    private void LateUpdate()
    {
        Move();
        AutoRotate();
        Rotate();
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


    private void AutoRotate()
    {
        if (isRight == true && autoRotMultiply < 1)
        {
            autoRotMultiply *= -1;
        }
        if (isRight == false && autoRotMultiply > -1)
        {
            autoRotMultiply *= -1;
        }

        autoRotSpeed = Mathf.Clamp(autoRotSpeed + (Time.deltaTime * autoRotMultiply), -rotClamp, rotClamp);
        var angle = lastRot.z - (autoRotSpeed);

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.rotation = Quaternion.Lerp(lastRot, rot, rotSpeed * Time.deltaTime);
        lastRot = spineTr.rotation;
    }

    private void Rotate()
    {
        var angle = Mathf.Clamp(lastRot.z - dir.x, -rotClamp, rotClamp);
        print("Angel: " + angle);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        spineTr.rotation = Quaternion.Lerp(lastRot, rot, rotSpeed * Time.deltaTime);
        lastRot = spineTr.rotation;
    }
}
