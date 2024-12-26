using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // Time before the bullet is destroyed
    public float damage = 10f; // Damage dealt by the bullet

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Destroy(gameObject, lifetime); // Destroy bullet after its lifetime
            Debug.Log("Bullet spawned!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet hit: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damage); // Apply damage
                Debug.Log($"Player hit! Damage dealt: {damage}");
            }
            Destroy(gameObject); // Destroy bullet after hitting the player
        }
        else
        {
            Destroy(gameObject); // Destroy bullet on other collisions
        }
    }
}
