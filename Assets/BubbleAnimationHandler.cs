using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleAnimationHandler : MonoBehaviour
{
    [SerializeField] private float animationTimeOnFrame = 2f;

    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    
    private void Start()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        foreach (var t in sprites)
        {
            image.sprite = t;
            yield return new WaitForSeconds(animationTimeOnFrame);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
