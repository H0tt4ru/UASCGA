using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;

void Start()
{
    Destroy(gameObject, lifetime);
    Debug.Log("Bullet spawned!");
}


    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10); // Example damage value
        }
        Destroy(gameObject); // Destroy bullet on impact
    }
    else
    {
        Destroy(gameObject); // Destroy bullet on other collisions
    }
    Debug.Log($"Bullet hit: {collision.gameObject.name}");
}

}
