using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;

public class ShootController : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    public Camera playerCamera;
    public float shootingRange = 100f;
    public Texture2D[] muzzleFlash;
    public float fireRate = 0.1f; // Time between shots
    private float nextTimeToFire = 0f;
    private int shotsFired = 0;
    private int maxShots = 30;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (shotsFired >= maxShots)
            {
                Debug.Log("Out of ammo! Press 'R' to reload.");
                return;
            }

            nextTimeToFire = Time.time + fireRate;
            Shoot();
            MuzzleFlash();
            shotsFired++;
        }

        ammoText.text = (maxShots - shotsFired).ToString("D2") + "/30";
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootingRange))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.OnHit(hit.point, playerCamera.transform.forward);
            }
        }
    }

    void MuzzleFlash()
    {
        // Implement muzzle flash logic here
    }

    void Reload()
    {
        shotsFired = 0;
        Debug.Log("Reloaded!");
    }
}
