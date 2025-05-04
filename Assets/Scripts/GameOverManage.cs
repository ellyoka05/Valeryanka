using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        FirstPersonController.OnPlayerDeath += ShowGameOver;
    }


    void OnDestroy()
    {
        FirstPersonController.OnPlayerDeath -= ShowGameOver;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}