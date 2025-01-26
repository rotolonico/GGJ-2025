using System.Collections;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private float horizontalLimit = 1.5f;
    [SerializeField] private float speed = 1.5f;
    
    private bool delay;
    
    private void Update()
    {
        if (!delay && (transform.localPosition.x > horizontalLimit || transform.localPosition.x < -horizontalLimit))
        {
            speed *= -1;
            delay = true;
            StartCoroutine(Delay());
        }
        
        transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
    }
    
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        delay = false;
    }
}
