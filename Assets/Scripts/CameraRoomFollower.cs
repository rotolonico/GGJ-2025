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
        var targetPosition = GetRoomClampedPosition(target.position, roomSize);
        
        var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
    
    public static Vector3 GetRoomClampedPosition(Vector3 position, Vector3 roomSize)
    {
        return new Vector3(
            Mathf.Round(position.x / roomSize.x) * roomSize.x,
            Mathf.Round(position.y / roomSize.y) * roomSize.y,
            position.z
        );
    }
}
