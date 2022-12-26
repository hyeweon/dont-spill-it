using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float forwardPower;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform spineTr;
    [SerializeField] private Transform spineTr2;

    private Vector3 firstTouch;
    private Vector3 lastTouch;
    private Vector3 dir;
    private bool isMove = false;

    private void Start()
    {
        spineTr2 = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }

    private void FixedUpdate()
    {
        
            Move();
    }

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

    void Rotate()
    {
        print("dir: " + dir.x);
        var moveVec = new Vector3(spineTr.rotation.x, spineTr.rotation.y, -dir.x);
        Vector3 pos = spineTr.rotation * (moveVec * speed * Time.deltaTime);
        spineTr.rotation = Quaternion.Euler(pos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
