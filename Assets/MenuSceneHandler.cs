using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI difficultyText;
    
    private void Update()
    {
        difficultyText.text = "Difficulty: " + DiffucltyManager.GetCurrentDifficulty();
    }
    
    public void ChangeDifficulty()
    {
        switch (DiffucltyManager.CurrentDifficulty)
        {
            case DiffucltyManager.Difficulty.Normal:
                DiffucltyManager.SetDifficulty(DiffucltyManager.Difficulty.Crazy);
                break;
            case DiffucltyManager.Difficulty.Crazy:
                DiffucltyManager.SetDifficulty(DiffucltyManager.Difficulty.Normal);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("BubbleScene");
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
