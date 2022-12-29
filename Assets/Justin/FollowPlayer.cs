using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform player;

    [SerializeField] private float zDis = -7f;
    [SerializeField] private float yDis = 5f;

    [SerializeField] private float endingPlusXvalue = 0;
    [SerializeField] private float endingPlusYvalue = 10;
    [SerializeField] private float endingPlusZvalue = 0;

    private float disZ = 0;
    private float disY = 0;

    [SerializeField] private Vector3 finalPos;
    [SerializeField] private bool isYFollowOnly = false;

    private void LateUpdate()
    {
        if (isYFollowOnly == false)
        {
            disZ = player.position.z + zDis; 
            disY = player.position.y + yDis;
            Vector3 target_pos = new Vector3(player.position.x, disY, disZ);
            transform.position = target_pos;
            transform.LookAt(target_pos);
        }

        if (isYFollowOnly == true)
        {
            Vector3 target_pos = new Vector3(finalPos.x + endingPlusXvalue, player.position.y + endingPlusYvalue, finalPos.z + endingPlusZvalue);
            transform.position = Vector3.Lerp(transform.position , target_pos, speed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform _target)
    {
        if (isYFollowOnly == false)
            isYFollowOnly = true;

        player = _target;
    }
}
