using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float forwardPower;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform spineTr;

    [SerializeField] private Renderer coffeeRen;


    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private Vector3 dir;
    private bool isMove = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstTouch.x = Input.mousePosition.x;
            isMove = true;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            lastTouch.x = Input.mousePosition.x;
            dir = lastTouch - firstTouch;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMove = false;
        }
    }

    private void LateUpdate()
    {
        Move();
        if (isMove)
        {
            Rotate();
        }
    }

    void Move()
    {
        var moveVec = new Vector3(0, 0, forwardPower);
        Vector3 pos = transform.position + (moveVec * speed * 0.005f * Time.deltaTime);
        transform.position = pos;
    }

    [SerializeField] private float rotSpeed; 
    [SerializeField] private float rotClamp;
    void Rotate()
    {
        var angle = (spineTr.rotation.z - dir.x);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        //print("Angle: " + (spineTr.rotation.z - dir.x));
        if(-rotClamp <= angle || rotClamp <= angle )
        {
            print("감소합니다. " + (coffeeRen.material.GetFloat("_Fill") - Time.deltaTime));
            coffeeRen.material.SetFloat("_Fill", coffeeRen.material.GetFloat("_Fill") - Time.deltaTime);
        }
        
        spineTr.rotation = Quaternion.Lerp(spineTr.rotation, rot, rotSpeed * Time.deltaTime);
    }
}
