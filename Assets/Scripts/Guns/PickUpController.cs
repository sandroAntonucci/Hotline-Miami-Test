using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PickUpController : MonoBehaviour
{
    [Header("PICK UP VARIABLES")]
    public float pickUpRange;
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;

    [SerializeField] private InputActionAsset PlayerControls;

    private BaseWeapon weaponScript;
    private Rigidbody rb;
    private BoxCollider coll;
    private Transform player, gunContainer, fpsCam;

    private InputAction pickAction;
    private InputAction dropAction;

    public bool equipped;
    public static bool slotFull;
    public static PickUpController weaponEquipped;
    public bool playerCanPick;

    private Ray cameraRay;

    private void Awake()
    {
        pickAction = PlayerControls.FindAction("PickUp");
        dropAction = PlayerControls.FindAction("Drop");

        pickAction.performed += _ =>
        {
            if(playerCanPick) PickUp();
        };
        dropAction.performed += _ =>
        {
            if(equipped) Drop();
        };
    }

    private void OnEnable()
    {
        pickAction.Enable();
        dropAction.Enable();
    }

    private void OnDisable()
    {
        pickAction.Disable();
        dropAction.Disable();
    }

    private void Start()
    {

        // Assign components
        weaponScript = GetComponent<BaseWeapon>();
        fpsCam = GameObject.Find("Main Camera").transform;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").transform;
        gunContainer = GameObject.Find("ItemHolder").transform;

        if (!equipped)
        {
            weaponScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            weaponScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
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

        // Disable physics
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        coll.isTrigger = true;

        // Start moving the gun smoothly
        StartCoroutine(MoveToGunContainer());

        // Enable script
        weaponScript.enabled = true;

        weaponEquipped = this;
    }

    private IEnumerator MoveToGunContainer()
    {
        float duration = 0.2f; // Adjust this for faster/slower movement
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, gunContainer.position, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(startRotation, gunContainer.rotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final position and rotation match perfectly
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }


    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        // Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        coll.isTrigger = false;

        // Carry player's momentum
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        // Add force to the throw
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random));

        // Disable script
        weaponScript.enabled = false;


        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>().isAiming)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAim>().ResetAim();
        }

        weaponEquipped = null;
    }

}
