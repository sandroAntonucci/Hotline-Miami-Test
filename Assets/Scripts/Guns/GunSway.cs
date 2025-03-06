using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSway : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Sway Settings")]
    public float swayAmount = 1.0f;   // How much the gun rotates
    public float maxSway = 5.0f;      // Max rotation angle
    public float smoothSpeed = 5.0f;  // Smoothing factor

    [Header("Bob Settings")]
    public float bobSpeed = 5f;
    public float bobAmount = 0.05f;
    public float returnSpeed = 2f; // Speed at which the gun returns to original position

    private Quaternion initialRotation;
    private float movementX;
    private float movementY;

    [SerializeField] private InputActionAsset PlayerControls;
    private InputAction lookAction;
    private InputAction moveAction;

    private Vector3 initialPosition;
    private float timer = 0f;
    private Vector2 lookInput;
    private Vector2 moveInput;

    private void Awake()
    {
        player = GameObject.Find("Player");
        lookAction = PlayerControls.FindActionMap("Player").FindAction("Look");
        moveAction = PlayerControls.FindActionMap("Player").FindAction("Move");

        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        lookAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
        moveAction.Disable();
    }

    private void Start()
    {
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        Sway();
        Quaternion targetRotation = Quaternion.Euler(movementY, -movementX, 0f) * initialRotation;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 velocity = new Vector3(moveInput.x, 0, moveInput.y);

        if (velocity.magnitude > 0.1f) // Small threshold to prevent unwanted bobbing
        {
            timer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(timer) * bobAmount;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + new Vector3(0, bobOffset, 0), Time.deltaTime * bobSpeed);
        }
        else
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
        }
    }

    private void Sway()
    {
        float mouseX = lookInput.x * 10;
        float mouseY = lookInput.y * 10;

        // Clamp values to avoid excessive rotation
        movementX = Mathf.Clamp(mouseX, -maxSway, maxSway);
        movementY = Mathf.Clamp(mouseY, -maxSway, maxSway);
    }
}