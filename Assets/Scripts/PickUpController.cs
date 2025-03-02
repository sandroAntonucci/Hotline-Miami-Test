using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PickUpController : MonoBehaviour
{
    [Header("PICK UP VARIABLES")]
    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;

    [SerializeField] private InputActionAsset PlayerControls;

    private BaseGun gunScript;
    private Rigidbody rb;
    private BoxCollider coll;
    private Transform player, gunContainer, fpsCam;

    private InputAction pickAction;
    private InputAction dropAction;

    private bool equipped;
    private static bool slotFull;
    public bool playerCanPick;

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
        gunScript = GetComponent<BaseGun>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        player = GameObject.Find("Player").transform;
        gunContainer = GameObject.Find("ItemHolder").transform;
        fpsCam = GameObject.Find("Main Camera").transform;

        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        // Check if player is in range and looking at the gun
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, fpsCam.forward * -1);

        if (Vector3.Distance(transform.position, player.position) < pickUpRange && angle < 30f && !slotFull && !equipped)
        {
            GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TextMeshProUGUI>().text = "[E] to pick up";
            playerCanPick = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("InteractionText").GetComponent<TextMeshProUGUI>().text = "";
            playerCanPick = false;
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Set position and parent of the gun
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        coll.isTrigger = true;

        // Enable script
        gunScript.enabled = true;

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
        gunScript.enabled = false;
    }

}
