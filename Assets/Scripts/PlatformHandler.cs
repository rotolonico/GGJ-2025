using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private float horizontalLimit = 1.5f;
    [SerializeField] private float speed = 1.5f;
    
    private void Update()
    {
        if (transform.localPosition.x > horizontalLimit)
        {
            transform.localPosition = new Vector3(-horizontalLimit, transform.localPosition.y, transform.localPosition.z);
        }
        
        transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
