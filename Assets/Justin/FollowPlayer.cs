using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private Transform player;

    [SerializeField] private bool isYFollowOnly = false;
    private float disZ = 0;

    private void LateUpdate()
    {
        if (isYFollowOnly == false)
        {
            disZ = player.position.z + -9.56f; 
            Vector3 target_pos = new Vector3(player.position.x, transform.position.y, disZ);
            transform.position = target_pos;
            transform.LookAt(target_pos);
        }

        if (isYFollowOnly == true)
        {
            Vector3 target_pos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target_pos, speed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform _target)
    {
        if (isYFollowOnly == false)
            isYFollowOnly = true;

        player = _target;
    }
}
