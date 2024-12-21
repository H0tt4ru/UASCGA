using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Animator animator;

    public bool ikActive = true; // Toggle IK
    public Transform rightHandTarget; // Target posisi tangan kanan (senjata)
    public Transform leftHandTarget;  // Target posisi tangan kiri (grip senjata)
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
    public AudioClip shootClip;

    private bool isShooting = false;

    [Header("Weapon Settings")]
    public GameObject weapon; // Drag the weapon prefab or object here in the inspector
    public Transform rightHandWeaponAttachment; // Specific part of the weapon that the right hand will hold (e.g., grip)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetRandomRoamTarget();
       

        if (weapon != null && rightHandWeaponAttachment != null)
        {
            // Instantiate the weapon and attach it to the right hand attachment
            GameObject weaponInstance = Instantiate(weapon, rightHandWeaponAttachment);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;
        }
           if (rightHandTarget != null && weapon != null && rightHandWeaponAttachment != null)
    {
        // Instantiate the weapon and attach it to the right hand attachment
        GameObject weaponInstance = Instantiate(weapon, rightHandWeaponAttachment);
        weaponInstance.transform.localPosition = Vector3.zero;
        
        // Correct the rotation if the weapon is pointing down
        // Adjust this based on your weapon model's original orientation
        weaponInstance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Adjust if necessary

        // Ensure the weapon faces forward (correct orientation)
        weaponInstance.transform.rotation = rightHandWeaponAttachment.rotation * Quaternion.Euler(90f, 0f, 0f); // Rotate by 90 degrees on the X axis

        // Attach the weapon to the right hand (position and orientation adjusted)
        weaponInstance.transform.parent = rightHandTarget;

        // Optional: Adjust the hand position slightly for the correct grip
        Vector3 gripOffset = rightHandTarget.position - weaponInstance.transform.position;
        weaponInstance.transform.position += gripOffset;
    }
    }

    void Update()
    {
        RaycastHit hit;
        // if (Physics.Raycast(transform.position, Vector3.down, out hit))
        // {
        //     if (hit.collider.CompareTag("Land"))
        //     {
        //         // Posisi objek disesuaikan dengan tanah
        //         transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        //     }
        // }

        if (agent != null && agent.enabled)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Set animator parameter when player is near
            if (distanceToPlayer <= detectionRange)
            {
               
                agent.SetDestination(player.position);

                if (!isShooting)
                {
                    StartCoroutine(ShootAtPlayer());
                }
            }
           

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                SetRandomRoamTarget();
            }

             if (rightHandTarget != null && weapon != null)
    {
        weapon.transform.position = rightHandTarget.position;
       
    }
    }
        

        
    }

    private IEnumerator ShootAtPlayer()
    {   
      
        isShooting = true;
        if (animator != null)
        {
            animator.SetBool("isShoot", true);
        }

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

            if (shootClip != null)
        {
            AudioSource.PlayClipAtPoint(shootClip, transform.position);
        }
        }

        yield return new WaitForSeconds(shootingInterval); // Wait for the interval before shooting again
     
        isShooting = false;
        if (animator != null)
        {
            animator.SetBool("isShoot", false);
        }
    }

    public void OnHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        Debug.Log($"Enemy hit at {hitPoint}! Current health: {health}");
        health -= 25;

        if (health <= 0)
        {   
             if (animator != null)
            {
                animator.SetBool("isDead", true); // Aktifkan animasi kematian
            }
            
            Debug.Log("Enemy destroyed!");
            if (animator != null)
            {
                animator.SetBool("isDead", true); // Aktifkan animasi kematian
            }
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
    void OnAnimatorIK(int layerIndex)
    {
        if (ikActive) // Ensure IK is active
        {
            // Setting the right hand target position and rotation using IK
            if (rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1); // Set position weight
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1); // Set rotation weight
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position); // Target right hand position
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation); // Target right hand rotation
            }

            // Setting the left hand target position and rotation using IK
            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1); // Set position weight
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1); // Set rotation weight
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position); // Target left hand position
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation); // Target left hand rotation
            }
        }
    }


    // private void OnDestroy()
    // {
    //     if (resource != null)
    //     {
    //         Instantiate(resource, transform.position, Quaternion.identity);
    //     }
    // }

    // void OnDisable()
    // {
    //     GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
    //     foreach (GameObject obj in resources)
    //     {
    //         Destroy(obj);
    //     }
    // }

    private void OnDestroy()
{
    if (resource != null)
    {
        // Buat resource di posisi musuh yang dihancurkan
        GameObject instantiatedResource = Instantiate(resource, transform.position, Quaternion.identity);

        // Beri nama dan tag untuk identifikasi
        instantiatedResource.name = "Resource_Clone";
        instantiatedResource.tag = "Resource";
    }
}

private void OnDisable()
{
    // Pastikan semua objek resource dihancurkan sebelum meninggalkan scene
    GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
    foreach (GameObject obj in resources)
    {
        Destroy(obj);
    }
}


}