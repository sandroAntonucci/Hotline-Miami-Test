using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableBoard : InteractableItem
{
    [SerializeField] private GameObject levelSelector;

    [SerializeField] private GameObject interactionCamera;

    [SerializeField] private CameraZoom cameraZoom;

    private GameObject player;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Interaction()
    {

        if (cameraZoom.isZooming) return;

        isInteracting = true;

        base.Interaction();

        cameraZoom.cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;

        player.SetActive(false);

        interactionCamera.SetActive(true);

        cameraZoom.ZoomIn();

        StartCoroutine(ActivateLevelSelector());

        interactionCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void StopInteraction()
    {
        if (cameraZoom.isZooming) return;

        levelSelector.SetActive(false);

        cameraZoom.ZoomOut();

        StartCoroutine(ResetInteraction());
    }

    private IEnumerator ResetInteraction()
    {
        while (cameraZoom.isZooming)
        {
            yield return null;
        }

        base.StopInteraction();

        isInteracting = false;

        player.SetActive(true);
        interactionCamera.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator ActivateLevelSelector()
    {
        while (cameraZoom.isZooming)
        {
            yield return null;
        }

        levelSelector.SetActive(true);
    }
}
