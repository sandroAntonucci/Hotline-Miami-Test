using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    public Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
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
        // Smoothly bring the gun back to its initial position
        back();

        if (currentRotation == targetRotation)
        {
            return;
        }

        // Decay recoil effect over time without forcing rotation to zero
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, Time.deltaTime * snapiness);

        // Apply recoil on top of PlayerRotate's existing rotation
        cameraHolder.localRotation *= Quaternion.Euler(currentRotation);

    }

    public void recoil(float recoil)
    {
        recoilX = recoil;

        // Apply recoil in the camera's local space, relative to the camera's current rotation
        Vector3 recoilDirection = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

        // Use cameraHolder instead of currentCameraRotation
        recoilDirection = cameraHolder.TransformDirection(recoilDirection);

        // Apply the recoil in world space (move the gun back)
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += cameraHolder.InverseTransformDirection(recoilDirection);

    }

    void back()
    {

        if (targetPosition == currentPosition)
        {
            return;
        }

        // Smoothly return the gun to its initial position
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snapiness);
        cameraHolder.transform.localPosition = currentPosition;
    }
}