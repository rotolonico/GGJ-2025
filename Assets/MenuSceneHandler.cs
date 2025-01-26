using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneHandler : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("TubeGeneratorExample");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
