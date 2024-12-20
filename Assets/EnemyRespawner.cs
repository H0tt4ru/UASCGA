using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab musuh yang akan di-spawn
    public Transform[] respawnPoints; // Array titik spawn
    public float initialSpawnInterval = 10f; // Interval awal spawn
    public float minSpawnInterval = 0.1f; // Interval minimum (0.1 detik)
    public float spawnAcceleration = 0.9f; // Faktor percepatan (0.9 = 10% lebih cepat setiap iterasi)

    private float currentSpawnInterval; // Interval spawn saat ini

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval; // Mulai dengan interval awal
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (currentSpawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            // Spawn musuh di titik acak
            Transform randomPoint = respawnPoints[Random.Range(0, respawnPoints.Length)];
            Instantiate(enemyPrefab, randomPoint.position, randomPoint.rotation);

            // Kurangi waktu spawn untuk mempercepat
            currentSpawnInterval *= spawnAcceleration; // Contoh: 10 detik -> 9 detik -> 8.1 detik ...
        }

        // Jika sudah mencapai interval minimum, mulai spawn 10 musuh dalam 1 detik
        StartCoroutine(SpawnMultipleEnemies());
    }

    private IEnumerator SpawnMultipleEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 10; i++) // Spawn 10 musuh
            {
                Transform randomPoint = respawnPoints[Random.Range(0, respawnPoints.Length)];
                Instantiate(enemyPrefab, randomPoint.position, randomPoint.rotation);
            }
        }
    }
}
