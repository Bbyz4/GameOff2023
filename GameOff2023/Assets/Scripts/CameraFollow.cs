using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float FollowSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float height;
    

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + height, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
