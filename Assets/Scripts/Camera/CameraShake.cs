using UnityEngine;

public class CameraStep : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform player;

    public float bobFrequency = 3f; // Speed of the bobbing effect
    public float bobAmplitude = 0.05f; // Height of the bobbing effect

    private float bobOffset = 0f;
    private float timer = 0f;

    private void Update()
    {
        HandleCameraBobbing();

        // Follow the player's height while applying bobbing
        transform.position = new Vector3(cameraPosition.position.x, player.position.y + bobOffset + 0.5f, cameraPosition.position.z);
    }

    private void HandleCameraBobbing()
    {
        if (player == null) return;

        float speed = player.GetComponent<CharacterController>().velocity.magnitude;

        if (speed > 0.05f) // Only bob if the player is moving
        {
            timer += Time.deltaTime * bobFrequency * speed; // Scale bobbing with speed
            bobOffset = Mathf.Sin(timer) * bobAmplitude;
        }
        else
        {
            bobOffset = Mathf.Lerp(bobOffset, 0f, Time.deltaTime);
        }
    }
}
