using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);

        // Subscribe to the player death event
        FirstPersonController.OnPlayerDeath += ShowGameOver;
    }

    // Unsubscribe to prevent memory leaks
    void OnDestroy()
    {
        FirstPersonController.OnPlayerDeath -= ShowGameOver;
    }

    public void ShowGameOver()
    {
        // Show the game over panel and pause the game
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // Unlock and show cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Reset time scale and reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        // Reset time scale and load main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}