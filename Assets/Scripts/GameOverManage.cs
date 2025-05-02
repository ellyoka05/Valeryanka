using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; 

    void Start()
    {
        gameOverPanel.SetActive(false); 
    }


    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 

        
        FirstPersonController player = FindObjectOfType<FirstPersonController>();
            if (player != null)
            {
                player.enabled = false; 
            }
        }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        if (player != null)
        {
            player.enabled = true;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); 
    }
}