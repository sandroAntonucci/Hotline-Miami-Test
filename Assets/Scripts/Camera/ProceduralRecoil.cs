using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, currentPosition, targetPosition, initialGunPosition;
    public Transform cameraHolder, currentCameraRotation;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float kickBackZ;

    public float snapiness = 10f;
    public float returnAmount = 5f;

    private void Start()
    {
        initialGunPosition = transform.localPosition; // Initialize starting position
    }

    private void Update()
    {
        // Smoothly return the rotation and position back to the initial state
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * snapiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        // Smoothly bring the gun back to its initial position
        //BackToInitialPosition();
    }

    public void Recoil(float recoilAmount)
    {
        recoilX = recoilAmount;

        // Apply recoil in the camera's local space
        Vector3 recoilDirection = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

        // Convert recoil to world space relative to the camera's rotation
        recoilDirection = currentCameraRotation.TransformDirection(recoilDirection);

        // Apply rotation recoil
        targetRotation += recoilDirection;

        // Apply positional recoil (kickback)
        targetPosition = initialGunPosition - new Vector3(0, 0, kickBackZ);
    }

    void BackToInitialPosition()
    {
        // Smoothly return the gun to its initial position
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snapiness);
        transform.localPosition = currentPosition;
    }
}

