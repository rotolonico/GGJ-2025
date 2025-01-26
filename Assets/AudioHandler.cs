using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance;
    
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    
    private bool isPlayingGameMusic;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayMusic(false);
    }

    private void PlayMusic(bool game)
    {
        if (game)
        {
            audioSource.clip = gameMusic;
            isPlayingGameMusic = true;
        }
        else
        {
            audioSource.clip = menuMusic;
            isPlayingGameMusic = false;
        }
        
        audioSource.Play();
    }

    private void Update()
    {
        if (isPlayingGameMusic && SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayMusic(false);
        } else if (!isPlayingGameMusic && SceneManager.GetActiveScene().name == "TubeGeneratorExample")
        {
            PlayMusic(true);
        }
    }
}
