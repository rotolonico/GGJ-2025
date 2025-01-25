using System.Collections;
using UnityEngine;

public class ValveHandler : MonoBehaviour
{
    private const float speed = 1f;
    
    private Quaternion startRotation;
    
    public void ActivateValve()
    {
        startRotation = transform.rotation;
        StartCoroutine(ActivateValveCoroutine());
    }
    
    private IEnumerator ActivateValveCoroutine()
    {
        // Do multiple 360 degrees lerp rotations then kill tbe object
        for (int i = 0; i < 3; i++)
        {
            var endRotation = Quaternion.Euler(0, 0, 360);
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                transform.localScale -= new Vector3(Time.deltaTime * speed * 20, Time.deltaTime * speed * 20, 0);
                transform.localScale = new Vector3(Mathf.Max(0, transform.localScale.x), Mathf.Max(0, transform.localScale.y), 0);
                yield return null;
            }
        }
        
        Destroy(gameObject);
    }
}
