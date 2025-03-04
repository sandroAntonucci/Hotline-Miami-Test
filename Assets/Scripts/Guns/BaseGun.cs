using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class BaseGun : MonoBehaviour
{
    public string gunName;

    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;

    public GameObject bulletPrefab;  // Bullet prefab is still needed for pool creation
    public Transform shootPosition;


    [SerializeField] private GameObject muzzleFlash;

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

        StartCoroutine(ShowMuzzleFlash());

        // Get a bullet from the pool
        GameObject bullet = BulletPool.Instance.GetBullet(shootPosition.position, shootPosition.rotation);

    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);

        // Randomize the rotation of the muzzle flash
        muzzleFlash.transform.localEulerAngles = new Vector3(Random.Range(0, 360), 180, 0);

        yield return new WaitForSeconds(0.1f);

        muzzleFlash.SetActive(false);
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
