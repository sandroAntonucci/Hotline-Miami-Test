using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Transform cameraPosition;
    public Transform objectiveCameraPosition;

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

            elapsedTime += Time.deltaTime;

            yield return null;
        }

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

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isZooming = false;
    }


}
