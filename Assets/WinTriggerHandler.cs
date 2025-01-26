using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene("BossFightTest");
        }
    }
}
