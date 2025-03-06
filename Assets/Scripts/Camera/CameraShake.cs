using UnityEngine;

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
        originalPosition = transform.localPosition; // Store the original local position of the camera
    }

    private void Update()
    {
        HandleCameraBobbing();
        
        transform.position = new Vector3(cameraPosition.position.x, cameraPosition.position.y + bobOffset, cameraPosition.position.z);
    }

    private void HandleCameraBobbing()
    {
        if (player == null) return;

        // Get player's movement speed (assumes Rigidbody movement)
        float speed = player.GetComponent<CharacterController>().velocity.magnitude;

        if (speed > 0.1f) // Only bob if the player is moving
        {
            timer += Time.deltaTime * bobFrequency * speed; // Scale bobbing with speed
            bobOffset = Mathf.Sin(timer) * bobAmplitude;
        }

        else
        {
            timer = 0f;
            bobOffset = Mathf.Lerp(bobOffset, 0f, Time.deltaTime * 2f); // Smoothly return to original position
        }

    }
}
