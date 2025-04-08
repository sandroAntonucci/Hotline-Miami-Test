using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpController : InteractableItem
{
    [Header("PICK UP VARIABLES")]
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;
    [SerializeField] private float throwForce = 600f;

    private BaseWeapon weaponScript;
    private Rigidbody rb;
    private BoxCollider coll;
    private Transform player, gunContainer, fpsCam;

    public bool equipped;
    public static bool slotFull;
    public static PickUpController weaponEquipped;

    private void Start()
    {
        weaponScript = GetComponent<BaseWeapon>();
        fpsCam = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").transform;
        gunContainer = GameObject.Find("ItemHolder").transform;

        weaponScript.enabled = false;
        rb.isKinematic = false;
        coll.isTrigger = false;

    }

    public override void Interaction()
    {
        if (!equipped && !isInteracting)
        {
            PickUp();
            base.Interaction();
        }
    }

    public override void StopInteraction()
    {
        if (equipped)
        {
            Drop();
            base.StopInteraction();
        }
    }

    private void PickUp()
    {
        if (weaponEquipped != null)
        {
            weaponEquipped.Drop();
        }

        equipped = true;
        slotFull = true;

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        coll.isTrigger = true;

        StartCoroutine(MoveToGunContainer());

        weaponScript.enabled = true;
        weaponEquipped = this;
    }

    private IEnumerator MoveToGunContainer()
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, gunContainer.position, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(startRotation, gunContainer.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        isInteracting = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.useGravity = true;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<CharacterController>().velocity;

        if (weaponScript.isMelee)
        {
            rb.AddForce(-transform.right * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(transform.up * dropUpwardForce / 2, ForceMode.Impulse);
            rb.AddTorque(transform.forward, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-transform.right * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);
            rb.AddTorque(Random.onUnitSphere);
        }

        weaponScript.enabled = false;

        var aim = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>();
        if (aim.isAiming)
        {
            Debug.Log("Resetting aim");
            aim.ResetAim();
        }

        weaponEquipped = null;
        isInteracting = false;
    }
}
