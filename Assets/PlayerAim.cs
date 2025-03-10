using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{

    [SerializeField] private InputActionAsset PlayerControls;

    [SerializeField] private GameObject itemHolder;

    [SerializeField] private GameObject marker;

    private InputAction aimAction;

    private GunSway gunSway;

    public bool isAiming = false;
    private bool isChangingAim = false;


    private void Awake()
    {
        aimAction = PlayerControls.FindActionMap("Player").FindAction("Aim");

        aimAction.performed += ctx => Aim();

        gunSway = itemHolder.GetComponent<GunSway>();
    }

    private void OnEnable()
    {
        aimAction.Enable();
    }

    private void OnDisable()
    {
        aimAction.Disable();
    }

    private void Aim()
    {

        if (isAiming) StartCoroutine(StopAim());
        else StartCoroutine(StartAim());

    }

    private IEnumerator StartAim()
    {
        marker.SetActive(false);
        gunSway.enabled = false;
        Vector3 targetPosition = new Vector3(0, -0.1f, 0.7f);

        // Instantly set rotation
        itemHolder.transform.localRotation = Quaternion.Euler(0, 90, 0);

        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.1f);
            yield return null;
        }

        isAiming = true;
        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set
    }

    private IEnumerator StopAim()
    {
        marker.SetActive(true);
        Vector3 targetPosition = new Vector3(0.25f, -0.2f, 0.4f);

        
        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.1f);
            yield return null;
        }

        isAiming = false;
        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set
        gunSway.enabled = true;
    }
    


}
