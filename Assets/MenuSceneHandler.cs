using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("TubeGeneratorExample");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
