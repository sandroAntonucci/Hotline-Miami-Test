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

    public float recoilStrength = 5f; // Adjust this value to control how strong the recoil is
    public float recoilRecoverySpeed = 1.0f;
    private Quaternion startingRotation = Quaternion.Euler(0f,0f,0f);
    private Quaternion targetRotation;


    [SerializeField] private GameObject muzzleFlash;

    [SerializeField] private Light shotLight;

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

        if (startingRotation != null)
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

        ApplyRecoil();
    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);

        // Randomize the rotation of the muzzle flash
        muzzleFlash.transform.localEulerAngles = new Vector3(Random.Range(0, 360), 180, 0);

        while(muzzleFlash.transform.localScale.x < 0.01)
        {
            shotLight.intensity += 0.6f;
            muzzleFlash.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            yield return null;
        }

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
        recoilStrength = fireRate * 30;
        recoilRecoverySpeed = 2 / fireRate;
        CameraEffects.Instance.recoil.recoil(-recoilStrength);
        float rand = Random.Range(-recoilStrength / 1.5f, recoilStrength / 1.5f);
        transform.rotation = transform.rotation * Quaternion.Euler(0f, rand, -recoilStrength);
    }

}
