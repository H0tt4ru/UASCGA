using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderScript : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if(obj.GetComponent<MeshRenderer>() != null && obj.GetComponent<MeshCollider>() == null)
            {
                obj.AddComponent<MeshCollider>();
            }
        }

        Debug.Log("Mesh Colliders added to all objects with Mesh Renderer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
