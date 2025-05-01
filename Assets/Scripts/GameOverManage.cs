using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in Inspector
    public static bool isGameOver = false;

    void Start()
    {
        gameOverPanel.SetActive(false); // Ensure panel starts disabled

        // Lock/hide cursor only if not game over
        if (!isGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // If game is already over (e.g., returning from pause), show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true; // Mark game as over

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze time

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;

        // Disable player movement
        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        if (player != null)
        {
            player.enabled = false;
        }

        Debug.Log("Game Over screen shown.");
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");

        isGameOver = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Debug.Log("Return to menu button clicked!");

        isGameOver = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu"); // Ensure the scene name matches
    }
}
