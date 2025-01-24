using UnityEngine;

public class CameraTestMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject cameraObject;
    
    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var direction = new Vector3(horizontal, vertical, 0);
        cameraObject.transform.position += direction * (speed * Time.deltaTime);
    }
}
