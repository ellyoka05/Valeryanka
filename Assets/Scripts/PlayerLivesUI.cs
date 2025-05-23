using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLivesUI : MonoBehaviour
{
    public int maxLives = 5;
    public Image[] lifeImages;
    public float respawnInvulnerabilityTime = 2f;

    private int currentLives;
    private FirstPersonController playerController;
    private bool isInvulnerable = false;


    void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();

        currentLives = maxLives;
        UpdateLivesUI();

        FirstPersonController.OnPlayerDeath += HandlePlayerDeath;

        
    }

    void OnDestroy()
    {
        if (playerController != null)
        {
            FirstPersonController.OnPlayerDeath -= HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        if (isInvulnerable) return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            print("Game Over!");
            currentLives = 5;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            StartCoroutine(InvulnerabilityPeriod());
        }
    }

    private System.Collections.IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(respawnInvulnerabilityTime);
        isInvulnerable = false;
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }
}