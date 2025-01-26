using System.Collections;
using UnityEngine;

public class ValveHandler : MonoBehaviour
{
    private const float speed = 1f;
    
    private Quaternion startRotation;

    [SerializeField] private Sprite solvedValve;
    [SerializeField] private SpriteRenderer sr;
    
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
                yield return null;
            }
        }
        
        sr.sprite = solvedValve;
    }
}
