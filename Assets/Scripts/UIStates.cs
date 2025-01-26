using UnityEngine;

public class UIStates : MonoBehaviour
{
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject gameOver;

    private void Start()
    {
        HUD.SetActive(true);
        gameOver.SetActive(false);

        PlayerHealthHandler.onDeath += () => gameOver.SetActive(true);
        PlayerHealthHandler.onDeath += () => HUD.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerHealthHandler.onDeath -= () => gameOver.SetActive(true);
        PlayerHealthHandler.onDeath -= () => HUD.SetActive(false);
    }
}
