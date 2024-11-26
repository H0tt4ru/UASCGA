using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ShootController : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    public Camera playerCamera;
    public float shootingRange = 100f;
    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        _inputs = transform.root.GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if(_inputs.shoot)
        {
            Shoot();
            _inputs.shoot = false;
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootingRange))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
