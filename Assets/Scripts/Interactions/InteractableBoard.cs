using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableBoard : InteractableItem
{

    [SerializeField] private GameObject interactionCamera;

    private Transform cameraPosition;
    public Transform objectiveCameraPosition;

    private bool isZooming = false;

    private GameObject player;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Interaction()
    {

        if (isZooming) return;

        isInteracting = true;

        base.Interaction();

        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;

        player.SetActive(false);

        interactionCamera.SetActive(true);

        StartCoroutine(CameraZoomIn());

        //interactionCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator CameraZoomIn()
    {

        isZooming = true;

        float elapsedTime = 0;
        float waitTime = 0.15f;

        while (elapsedTime < waitTime)
        {
            interactionCamera.transform.position = Vector3.Lerp(cameraPosition.position, objectiveCameraPosition.position, (elapsedTime / waitTime));
            interactionCamera.transform.rotation = Quaternion.Lerp(cameraPosition.rotation, objectiveCameraPosition.rotation, (elapsedTime / waitTime));

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
            interactionCamera.transform.position = Vector3.Lerp(objectiveCameraPosition.position, cameraPosition.position, (elapsedTime / waitTime));
            interactionCamera.transform.rotation = Quaternion.Lerp(objectiveCameraPosition.rotation, cameraPosition.rotation, (elapsedTime / waitTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        player.SetActive(true);
        interactionCamera.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isZooming = false;
    }

    public override void StopInteraction()
    {
        if (isZooming) return;

        isInteracting = false;

        base.StopInteraction();
        StartCoroutine(CameraZoomOut());
    }

}
