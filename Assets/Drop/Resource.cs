using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private float temp;

    // Start is called before the first frame update
    void Awake()
    {
        temp = transform.position.y;
    }

    void Update()
    {
        // Rotate the object slowly
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);

        // Move the object up and down
        float newY = Mathf.Sin(Time.time) * 0.2f;
        transform.position = new Vector3(transform.position.x, temp + newY, transform.position.z);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeHealth();
            Destroy(gameObject);
        }
    }
}
