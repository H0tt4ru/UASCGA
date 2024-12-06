using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float knockbackForce = 10f;
    public int health = 100;
    public Transform player;
    public float detectionRange = 10f;
    public float roamRange = 15f;

    private NavMeshAgent agent;
    private Vector3 roamTarget;
    public GameObject resource;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Assign the bullet prefab here
    public Transform shootingPoint; // Where bullets will spawn
    public float shootingInterval = 2f; // Time between shots
    public float bulletSpeed = 10f;

    private bool isShooting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomRoamTarget();
    }

    void Update()
    {
        if (agent != null && agent.enabled)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                agent.SetDestination(player.position);

                if (!isShooting)
                {
                    StartCoroutine(ShootAtPlayer());
                }
            }
            else if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                SetRandomRoamTarget();
            }
        }
    }

    private IEnumerator ShootAtPlayer()
{
    isShooting = true;

    if (bulletPrefab != null && shootingPoint != null)
    {
        // Spawn the bullet at the shooting point
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);

        // Apply force to move the bullet toward the player
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (player.position - shootingPoint.position).normalized; // Calculate direction
            rb.velocity = direction * bulletSpeed; // Propel the bullet
        }

        // Destroy the bullet after 5 seconds to prevent clutter
        Destroy(bullet, 5f);
    }

    yield return new WaitForSeconds(shootingInterval); // Wait for the interval before shooting again
    isShooting = false;
}



    public void OnHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        Debug.Log($"Enemy hit at {hitPoint}! Current health: {health}");
        health -= 25;

        if (health <= 0)
        {
            Debug.Log("Enemy destroyed!");
            Destroy(gameObject);
        }
        else
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode.Impulse);
            }

            StartCoroutine(ReEnableNavMeshAgent(0.2f, navMeshAgent, rb));
        }
    }

    private IEnumerator ReEnableNavMeshAgent(float delay, NavMeshAgent agent, Rigidbody rb)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        if (agent != null)
        {
            agent.enabled = true;
        }
    }

    private void SetRandomRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRange;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRange, 1))
        {
            roamTarget = hit.position;
            agent.SetDestination(roamTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRange);
    }
    private void OnDestroy() {
        if (resource != null) {
            Instantiate(resource, transform.position, Quaternion.identity);
        }
    }
}
