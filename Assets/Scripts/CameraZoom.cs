using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Transform cameraPosition;
    public Transform objectiveCameraPosition;

    [SerializeField] private int startingFOV;
    [SerializeField] private float endingFOV;

    [SerializeField] bool changesFOV;


    [SerializeField] private Camera zoomCamera;
    public bool isZooming = false;

    

    public void ZoomIn()
    {
        StartCoroutine(CameraZoomIn());
    }

    public void ZoomOut()
    {
        StartCoroutine(CameraZoomOut());
    }

    private IEnumerator CameraZoomIn()
    {

        isZooming = true;

        float elapsedTime = 0;
        float waitTime = 0.15f;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(cameraPosition.position, objectiveCameraPosition.position, (elapsedTime / waitTime));
            transform.rotation = Quaternion.Lerp(cameraPosition.rotation, objectiveCameraPosition.rotation, (elapsedTime / waitTime));

            if (changesFOV) zoomCamera.fieldOfView = Mathf.Lerp(startingFOV, endingFOV, (elapsedTime / waitTime)); 

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = objectiveCameraPosition.position;
        transform.rotation = objectiveCameraPosition.rotation;

        if (changesFOV) zoomCamera.fieldOfView = endingFOV;

        isZooming = false;
    }

    private IEnumerator CameraZoomOut()
    {

        isZooming = true;

        float elapsedTime = 0;
        float waitTime = 0.15f;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(objectiveCameraPosition.position, cameraPosition.position, (elapsedTime / waitTime));
            transform.rotation = Quaternion.Lerp(objectiveCameraPosition.rotation, cameraPosition.rotation, (elapsedTime / waitTime));

            if (changesFOV) zoomCamera.fieldOfView = Mathf.Lerp(endingFOV, startingFOV, (elapsedTime / waitTime));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;

        if (changesFOV) zoomCamera.fieldOfView = startingFOV;

        isZooming = false;
    }


}
