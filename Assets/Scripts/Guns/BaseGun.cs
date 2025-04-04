using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class BaseGun : BaseWeapon
{
    public string gunName;

    public Vector3 aimPosition;

    public bool isEnemyGun = false;

    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public int weaponBulletDamage = 100;

    public GameObject bulletPrefab;  // Bullet prefab is still needed for pool creation
    public Transform shootPosition; 

    public float recoilStrength = 5f; // Adjust this value to control how strong the recoil is
    public float recoilRecoverySpeed = 1.0f;
    private Quaternion startingRotation = Quaternion.Euler(0f,0f,0f);


    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private Light shotLight;

    public InputActionAsset PlayerControls;
    private InputAction shootAction;

    public bool canShoot = true;
    private bool isShooting = false;
    public float recoilStrengthMultiplier = 30;

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

    public void Update()
    {
        if (isShooting && canShoot && currentAmmo > 0)
        {
            Shoot();
        }

        if (startingRotation != null && !isEnemyGun)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startingRotation, Time.deltaTime * recoilRecoverySpeed);
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
        BaseBullet bulletComp = bullet.GetComponent<BaseBullet>();

        if (isEnemyGun) bulletComp.isEnemyBullet = true;
        else bulletComp.isEnemyBullet = false;

        bulletComp.bulletDamage = weaponBulletDamage;

        bullet.SetActive(true);

        if(!isEnemyGun) ApplyRecoil();
    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);

        // Randomize the rotation of the muzzle flash
        muzzleFlash.transform.localEulerAngles = new Vector3(Random.Range(0, 360), 180, 0);

        while(muzzleFlash.transform.localScale.x < 0.01)
        {
            shotLight.intensity += 0.3f;
            muzzleFlash.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        while (muzzleFlash.transform.localScale.x > 0)
        {
            shotLight.intensity -= 0.3f;
            muzzleFlash.transform.localScale -= new Vector3(0.004f, 0.004f, 0.004f);
            yield return null;
        }

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

    private void ApplyRecoil()
    {
        recoilStrength = fireRate * recoilStrengthMultiplier; 
        recoilRecoverySpeed = 2 / fireRate;
        CameraEffects.Instance.recoil.recoil(-recoilStrength);
        float rand = Random.Range(-recoilStrength / 3f, recoilStrength / 3f);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, rand, -recoilStrength);
    }

}
