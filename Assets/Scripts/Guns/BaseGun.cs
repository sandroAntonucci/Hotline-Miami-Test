using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public abstract class BaseGun : MonoBehaviour
{
    public string gunName;

    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;

    public GameObject bulletPrefab;  // Bullet prefab is still needed for pool creation
    public Transform shootPosition;

    public InputActionAsset PlayerControls;
    private InputAction shootAction;

    private bool canShoot = true;
    private bool isShooting = false;

    private Coroutine ShootCooldownCoroutine;

    private void Awake()
    {
        shootAction = PlayerControls.FindAction("Shoot");
        shootAction.performed += ctx => StartShooting();
        shootAction.canceled += ctx => StopShooting();
    }

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public virtual void Update()
    {
        if (isShooting && canShoot && currentAmmo > 0)
        {
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        if (ShootCooldownCoroutine != null)
        {
            StopCoroutine(ShootCooldownCoroutine);
        }

        ShootCooldownCoroutine = StartCoroutine(ShootCooldown());

        currentAmmo--;

        // Get a bullet from the pool
        GameObject bullet = BulletPool.Instance.GetBullet(shootPosition.position, shootPosition.rotation);

    }

    public virtual void StartShooting()
    {
        isShooting = true;
    }

    public virtual void StopShooting()
    {
        isShooting = false;
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
