using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{

    [SerializeField] private InputActionAsset PlayerControls;

    [SerializeField] private GameObject itemHolder;

    private InputAction aimAction;

    private void Awake()
    {
        aimAction = PlayerControls.FindActionMap("Player").FindAction("Aim");

        aimAction.performed += ctx => StartCoroutine(Aim());
    }

    private void OnEnable()
    {
        aimAction.Enable();
    }

    private void OnDisable()
    {
        aimAction.Disable();
    }

    private IEnumerator Aim()
    {

        // Lerp gun position to the center of the camera
        while (Vector3.Distance(itemHolder.transform.position, new Vector3(0f, -0.1f, 0.5f)) > 0.01f)
        {
            itemHolder.transform.position = Vector3.Lerp(itemHolder.transform.position, new Vector3(0f, -0.1f, 0.5f), Time.deltaTime * 10);
            yield return null;
        }



    }

}
