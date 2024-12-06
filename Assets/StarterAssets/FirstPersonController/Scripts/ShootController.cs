using System.Collections;
using UnityEngine;
using TMPro;

public class ShootController : MonoBehaviour
{
    public Camera playerCamera;
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float BulletSpeed = 100;
    private int Ammo = 30;
    private int MaxAmmo = 30;
    private int ReserveAmmo = 90;
    private bool Reloading = false;
    private float ReloadTime = 1.5f;
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI ReserveAmmoText;

    // private Animator Animator;
    private float LastShootTime;

    private void Awake()
    {
        // Animator = GetComponent<Animator>();
        ShootingSystem = transform.Find("MuzzleFlash01").GetComponent<ParticleSystem>();
        UpdateAmmoUI();
    }

    private void Update() {
        if (Input.GetMouseButton(0) && !Reloading) {
            Shoot();
        } else {
            ShootingSystem.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R) && !Reloading) {
            StartCoroutine(Reload());
        }
    }

    public void AddAmmo()
    {
        Debug.Log("Player has picked up ammo!");
        ReserveAmmo = 90;
        UpdateAmmoUI();
    }

    private void Shoot()
    {
        if (Ammo <= 0) {
            ShootingSystem.Stop();
            return;
        }

        ShootingSystem.Play();
        if (LastShootTime + ShootDelay < Time.time) {
            // Animator.SetBool("IsShooting", true);
            Vector3 direction = GetDirection();
        
            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask)) {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.OnHit(hit.point, playerCamera.transform.forward);
                }
            } else {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, Vector3.zero, false));
            }

            LastShootTime = Time.time;
            Ammo--;
            UpdateAmmoUI();
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        if (AddBulletSpread) {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );
            direction.Normalize();
        }
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(startPosition, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0) {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= BulletSpeed * Time.deltaTime;
            yield return null;
        }

        // Animator.SetBool("IsShooting", false);
        trail.transform.position = hitPoint;
        if (madeImpact) {
            Instantiate(ImpactParticleSystem, hitPoint, Quaternion.LookRotation(hitNormal));
        }

        Destroy(trail.gameObject, trail.time);
    }

    private IEnumerator Reload()
    {
        if (Ammo == MaxAmmo || ReserveAmmo <= 0) {
            yield break;
        }

        Reloading = true;
        // Animator.SetBool("IsReloading", true);
        yield return new WaitForSeconds(ReloadTime);

        int ammoToReload = MaxAmmo - Ammo;
        if (ReserveAmmo >= ammoToReload) {
            Ammo += ammoToReload;
            ReserveAmmo -= ammoToReload;
        } else {
            Ammo += ReserveAmmo;
            ReserveAmmo = 0;
        }

        Reloading = false;
        // Animator.SetBool("IsReloading", false);
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        AmmoText.text = Ammo.ToString();
        ReserveAmmoText.text = ReserveAmmo.ToString();
    }
}
