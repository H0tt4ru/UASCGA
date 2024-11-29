using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float knockbackForce = 10f;
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to be called when the enemy is hit by a raycast
    public void OnHit(Vector3 hitPoint, Vector3 hitDirection)
    {
        health -= 25;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(hitDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
