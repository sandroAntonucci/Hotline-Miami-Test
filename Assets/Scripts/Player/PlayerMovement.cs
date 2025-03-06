using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float upDownRange = 80f;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 0.1f;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset PlayerControls;

    [Header("Sliding")]
    [SerializeField] private float slideDuration = 1f; // Duration of the slide
    [SerializeField] private float slideSpeedMultiplier = 2f; // Speed multiplier during the slide
    [SerializeField] private float slideHeight = 0.5f; // Height of the character controller during the slide
    [SerializeField] private float slideCooldown = 2f; // Cooldown before the player can slide again


    private float originalSlideSpeedMultiplier;

    private float originalHeight; 
    private bool isSliding = false; 
    private float slideTimer = 0f; 
    private float slideCooldownTimer = 0f;

    private Vector3 targetMovement = Vector3.zero;
    private Vector3 currentVelocity = Vector3.zero;
    private float verticalVelocity; 

    private int lastPlayedIndex = -1;
    private bool isMoving;
    private float nextStepTime;
    private Camera mainCamera;
    public float verticalRotation;
    private Vector3 currentMovement = Vector3.zero;
    private CharacterController characterController;

    private GameObject itemHolder;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private InputAction slideAction;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        originalHeight = gameObject.transform.localScale.y;
        originalSlideSpeedMultiplier = slideSpeedMultiplier;
        itemHolder = GameObject.FindGameObjectWithTag("ItemHolder");

        moveAction = PlayerControls.FindActionMap("Player").FindAction("Move");
        lookAction = PlayerControls.FindActionMap("Player").FindAction("Look");
        sprintAction = PlayerControls.FindActionMap("Player").FindAction("Sprint");
        slideAction = PlayerControls.FindActionMap("Player").FindAction("Slide");

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;

        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => lookInput = Vector2.zero;

        slideAction.performed += ctx => StartSlide();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        slideAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        slideAction.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        //HandleFootsteps();
    }

    // Player movement
    private void HandleMovement()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            slideSpeedMultiplier = originalSlideSpeedMultiplier * (slideTimer / slideDuration) * 2;
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
        else if (slideCooldownTimer > 0f)
        {
            slideCooldownTimer -= Time.deltaTime;
        }

        float speedMultiplier = sprintAction.ReadValue<float>() > 0.1f ? sprintMultiplier : 1f;

        if (isSliding)
        {
            speedMultiplier *= slideSpeedMultiplier;
        }

        targetMovement = new Vector3(moveInput.x, 0, moveInput.y) * walkSpeed * speedMultiplier;
        targetMovement = transform.rotation * targetMovement;

        // Movement acceleration / deceleration
        currentMovement.x = Mathf.Lerp(currentMovement.x, targetMovement.x, acceleration * Time.deltaTime);
        currentMovement.z = Mathf.Lerp(currentMovement.z, targetMovement.z, acceleration * Time.deltaTime);

        // Gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * 2;
        }

        currentMovement.y = verticalVelocity;
        characterController.Move(currentMovement * Time.deltaTime);
        isMoving = moveInput.sqrMagnitude > 0;

        mainCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
    }

    // Player rotation / camera look
    private void HandleRotation()
    {
        float mouseXRotation = lookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, gameObject.transform.localRotation.eulerAngles.y, 0);
    }

    private void StartSlide()
    {
        if (!isSliding && slideCooldownTimer <= 0f && isMoving)
        {
            isSliding = true;
            slideTimer = slideDuration;

            // Start a coroutine to smoothly transition to the slide height
            StartCoroutine(LerpHeight(originalHeight, slideHeight, 0.2f));
            StartCoroutine(CameraEffects.Instance.ChangeFOV(90f, 0.2f));
        }
    }

    private void StopSlide()
    {
        if (isSliding)
        {
            isSliding = false;
            slideCooldownTimer = slideCooldown;
            slideSpeedMultiplier = originalSlideSpeedMultiplier;

            // Start a coroutine to smoothly transition back to the original height
            StartCoroutine(LerpHeight(slideHeight, originalHeight, 0.2f)); // Adjust the duration (0.2f) as needed
            StartCoroutine(CameraEffects.Instance.ChangeFOV(70f, 0.2f));
        }
    }

    private IEnumerator LerpHeight(float fromHeight, float toHeight, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate the height
            float newHeight = Mathf.Lerp(fromHeight, toHeight, elapsedTime / duration);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, newHeight, gameObject.transform.localScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final height is set exactly
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, toHeight, gameObject.transform.localScale.z);
    }

    /*
    private void HandleFootsteps()
    {
        float currentStepInterval = sprintAction.ReadValue<float>() > 0 ? sprintStepInterval : walkStepInterval;

        if (characterController.isGrounded && isMoving && Time.time > nextStepTime && characterController.velocity.magnitude > velocityThreshold)
        {
            nextStepTime = Time.time + currentStepInterval;
            cameraMovement.StepCamera();
            //PlayFootstepSounds();
        }
    }*/
    

    /* Play a random footstep sound 
    private void PlayFootstepSounds()
    {

        int randomIndex;

        if (footstepSounds.Length == 1)
        {
            randomIndex = 0;
        }
        else
        {
            randomIndex = Random.Range(0, footstepSounds.Length-1);
            if (randomIndex >= lastPlayedIndex)
            {
                randomIndex = footstepSounds.Length - 1;
            }
        }

        lastPlayedIndex = randomIndex;
        footstepSource.clip = footstepSounds[randomIndex];
        footstepSource.Play();
    }
    */

}
