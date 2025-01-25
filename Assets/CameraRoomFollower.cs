using System;
using UnityEngine;

public class CameraRoomFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 roomSize;
    
    private void Update()
    {
        // Round the target position to the nearest room size
        var targetPosition = new Vector3(
            Mathf.Round(target.position.x / roomSize.x) * roomSize.x,
            Mathf.Round(target.position.y / roomSize.y) * roomSize.y,
            target.position.z
        );
        
        var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
