using UnityEngine;

public class AnchorInstance : MonoBehaviour
{
    public static Vector3 AnchorPosition;
    
    private void Start()
    {
        AnchorPosition = transform.position;
    }
}
