using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            Debug.Log("Player entered the bound");

            // Call TakeDamage on PlayerHealth and give it 9001 damage
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(9001);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on Player!");
            }
        }
    }
}
