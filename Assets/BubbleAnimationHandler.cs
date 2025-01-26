using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleAnimationHandler : MonoBehaviour
{
    [SerializeField] private float animationTimeOnFrame = 1f;
    
    [SerializeField] private Image[] images;
    
    private void Start()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        foreach (var i in images)
        {
            i.gameObject.SetActive(true);
            yield return new WaitForSeconds(animationTimeOnFrame);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
