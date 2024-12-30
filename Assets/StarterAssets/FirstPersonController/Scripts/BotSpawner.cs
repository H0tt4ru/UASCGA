using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour
{
    public GameObject botPrefab;  // Prefab for the bot
    public float initialSpawnRate = 5f;  // Initial spawn rate (in seconds)
    public float spawnRateDecreasePerMinute = 0.1f;  // How much to decrease spawn rate every minute

    private float currentSpawnRate;
    private float timeElapsed = 0f;
    private bool isSpawning = true;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(SpawnBots());
    }

    void Update()
    {
        // Update elapsed time and decrease spawn rate as time progresses
        timeElapsed += Time.deltaTime;

        // Every ten seconds, decrease the spawn rate
        if (timeElapsed >= 10f)
        {
            timeElapsed = 0f;  // Reset the timer every ten seconds
            DecreaseSpawnRate();
        }
    }

    // Coroutine that spawns bots at intervals
    IEnumerator SpawnBots()
    {
        while (isSpawning)
        {
            SpawnBot();
            yield return new WaitForSeconds(currentSpawnRate);  // Wait for the current spawn rate
        }
    }

    // Spawn a bot at the spawner's position

    void SpawnBot()
    {
        // Generate a random position within a radius
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * 40f;
        randomPosition.y = transform.position.y;  // Keep the same height

        // Instantiate the bot at the random position and spawner's rotation
        Instantiate(botPrefab, randomPosition, transform.rotation);

        // Log that a new bot has spawned
        Debug.Log("New bot spawned at: " + randomPosition);
    }

    // Decrease spawn rate
    void DecreaseSpawnRate()
    {
        currentSpawnRate -= spawnRateDecreasePerMinute;
        if (currentSpawnRate < 0.1f)
        {
            currentSpawnRate = 0.1f;
        }
    }
}
