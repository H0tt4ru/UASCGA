using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For displaying health on the UI

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    private int currentHealth; // Current health of the player
    public Text healthText; // Reference to a UI Text element to display health
    public GameObject gameOverScreen; // Reference to a Game Over UI panel

    void Start()
    {
        currentHealth = maxHealth; // Initialize player's health
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        // Check if the player's health reaches zero
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        // Update the UI text to show the current health
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Show Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Disable player controls (if applicable)
        // Example: GetComponent<PlayerMovement>().enabled = false;

        // Optionally restart the game or load a specific scene after a delay
        StartCoroutine(RestartGame(3f));
    }

    private IEnumerator RestartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
