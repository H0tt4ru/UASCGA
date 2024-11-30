using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float knockbackForce = 10f;
    public int health = 100;
    public Transform player; // Drag the player's transform here in the Inspector
    public float detectionRange = 10f; // Range to detect the player
    public float roamRange = 15f; // Range within which the enemy roams

    private NavMeshAgent agent;
    private Vector3 roamTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomRoamTarget();
    }

    // Update is called once per frame
    void Update()
{
    if (agent != null && agent.enabled) // Check if NavMeshAgent is active
    {
        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Chase the player
            agent.SetDestination(player.position);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Continue roaming
            SetRandomRoamTarget();
        }
    }
}


    // Method to be called when the enemy is hit by a raycast
    public void OnHit(Vector3 hitPoint, Vector3 hitDirection)
{
    Debug.Log($"Enemy hit at {hitPoint}! Current health: {health}");
    health -= 25; // Decrease health by 25 per hit

    if (health <= 0)
    {
        Debug.Log("Enemy destroyed!");
        Destroy(gameObject);
    }
    else
    {
        // Temporarily disable NavMeshAgent to allow knockback
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Temporarily disable kinematic
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode.Impulse);
        }

        // Re-enable NavMeshAgent after a delay
        StartCoroutine(ReEnableNavMeshAgent(0.2f, navMeshAgent, rb));
    }
}

private IEnumerator ReEnableNavMeshAgent(float delay, NavMeshAgent agent, Rigidbody rb)
{
    yield return new WaitForSeconds(delay);

    if (rb != null)
    {
        rb.isKinematic = true; // Reset to kinematic for NavMeshAgent
    }

    if (agent != null)
    {
        agent.enabled = true; // Re-enable NavMeshAgent
    }
}


    // Sets a random target for roaming
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

    // Optional: Visualize detection and roam ranges in the Editor
    private void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Roam range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRange);
    }
}
