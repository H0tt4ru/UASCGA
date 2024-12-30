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

        // Every minute, decrease the spawn rate
        if (timeElapsed >= 60f)
        {
            timeElapsed = 0f;  // Reset the timer every minute
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
        // Instantiate the bot at the spawner's position and rotation
        Instantiate(botPrefab, transform.position, transform.rotation);

        // Log that a new bot has spawned
        Debug.Log("New bot spawned at: " + transform.position);
    }

    // Decrease spawn rate
    void DecreaseSpawnRate()
    {
        currentSpawnRate = Mathf.Max(0.5f, currentSpawnRate - spawnRateDecreasePerMinute);
    }
}
