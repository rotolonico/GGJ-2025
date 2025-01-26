using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleAnimationHandler : MonoBehaviour
{
    [SerializeField] private float animationTimeOnFrame = 1f; // Time to fully display each image
    [SerializeField] private float dissolveDuration = 0.5f;    // Duration of dissolve effect
    [SerializeField] private Image[] images;                  // Images to animate

    private void Start()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        foreach (var image in images)
        {
            // Start dissolve effect
            yield return StartCoroutine(DissolveIn(image));
            // Wait for the specified time before moving to the next image
            yield return new WaitForSeconds(animationTimeOnFrame);
        }

        // Load the next scene
        SceneManager.LoadScene("TubeGeneratorExample");
    }

    private IEnumerator DissolveIn(Image image)
    {
        image.enabled = true; // Make sure the image is visible
        Color color = image.color;
        color.a = 0f; // Start with transparent
        image.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < dissolveDuration)
        {
            // Gradually increase alpha over time
            color.a = Mathf.Clamp01(elapsedTime / dissolveDuration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the alpha is fully set to 1
        color.a = 1f;
        image.color = color;
    }
}