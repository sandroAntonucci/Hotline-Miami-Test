using UnityEngine;
using System.Collections;

public class CameraStep : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform player;

    public float bobFrequency = 3f; // Speed of the bobbing effect
    public float bobAmplitude = 0.05f; // Height of the bobbing effect

    float bobOffset = 0f;

    private float timer = 0f;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = cameraPosition.position; // Store the original local position of the camera
    }

    private void Update()
    {
        HandleCameraBobbing();

        // Ensure camera stays relative to original height
        transform.position = new Vector3(cameraPosition.position.x, originalPosition.y + bobOffset, cameraPosition.position.z);
    }


    private void HandleCameraBobbing()
    {
        if (player == null) return;

        // Get player's movement speed (assumes Rigidbody movement)
        float speed = player.GetComponent<CharacterController>().velocity.magnitude;

        if (speed > 0.05f) // Only bob if the player is moving
        {
            timer += Time.deltaTime * bobFrequency * speed; // Scale bobbing with speed
            bobOffset = Mathf.Sin(timer) * bobAmplitude;
        }

        else if (!player.GetComponent<PlayerMovement>().isMoving)
        {
            bobOffset = Mathf.Lerp(bobOffset, 0f, Time.deltaTime);
        }

    }

}
