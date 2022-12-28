using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float zPos = 0;
    [SerializeField] private float yPos = 0;

    [SerializeField] private Transform targetTR;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = targetTR;
        zPos = targetTR.position.z - transform.position.z;
        yPos = targetTR.position.y - transform.position.y;
    }

    private void LateUpdate()
    {
        Vector3 target_pos = new Vector3(targetTR.position.x, targetTR.position.y - yPos, targetTR.position.z - zPos);
        transform.position = Vector3.Lerp(transform.position, target_pos, speed * Time.deltaTime);
    }

    //카메라가 따라갈 오브젝트 변경
    public void changeTarget(Transform target)
    {
        targetTR = target;
    }

    //카메라가 플레이어를 따라오게 변경
    public void targetToPlayer()
    {
        targetTR = playerTransform;
    }
}