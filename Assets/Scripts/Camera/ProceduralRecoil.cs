using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    public Transform cameraHolder, currentCameraRotation;

    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float kickBackZ;

    public float snapiness, returnAmount;

    private void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        // Smoothly return the rotation and position back to the initial state
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * snapiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        // Smoothly bring the gun back to its initial position
        //back();
    }

    public void recoil(float recoil)
    {
        recoilX = recoil;

        // Apply recoil in the camera's local space, relative to the camera's current rotation
        Vector3 recoilDirection = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

        // Apply the recoil based on the camera's orientation (currentCameraRotation is used for local space conversion)
        recoilDirection = currentCameraRotation.TransformDirection(recoilDirection);  // Convert recoil to world space relative to the camera

        // Apply the recoil in world space (move the gun back)
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += recoilDirection;

    }

    void back()
    {
        // Smoothly return the gun to its initial position
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snapiness);
        transform.localPosition = currentPosition;
    }
}