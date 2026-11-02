using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public GameObject startMenu;

    private bool gameStarted = false;

    public void StartGame()
    {
        if (gameStarted) return;

        gameStarted = true;

        startMenu.SetActive(false);

        waveSpawner.StartWaves();
    }
}
