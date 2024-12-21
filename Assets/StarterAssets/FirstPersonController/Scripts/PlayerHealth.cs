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

    public AudioClip damageClip; // Audio clip to play when taking damage
    private AudioSource audioSource; // AudioSource component

    void Start()
    {
        currentHealth = maxHealth; // Initialize player's health
        UpdateHealthUI();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damageClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageClip);
        }
        
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

    public void TakeHealth()
    {
        Debug.Log("Player has picked up health!");
        // Increase the player's health
        currentHealth = maxHealth; // Restore to max health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();

        // Access the ShootController on AR_A inside PlayerCameraRoot
        Transform playerCameraRoot = transform.Find("PlayerCameraRoot");
        if (playerCameraRoot != null)
        {
            Transform arATransform = playerCameraRoot.Find("AR_A");
            if (arATransform != null)
            {
                ShootController shootController = arATransform.GetComponent<ShootController>();
                if (shootController != null)
                {
                    shootController.AddAmmo();
                }
                else
                {
                    Debug.LogWarning("ShootController component not found on AR_A!");
                }
            }
            else
            {
                Debug.LogWarning("AR_A object not found under PlayerCameraRoot!");
            }
        }
        else
        {
            Debug.LogWarning("PlayerCameraRoot object not found!");
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
        RestartGame();
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
