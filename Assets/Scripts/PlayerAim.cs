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

    private Coroutine aimCoroutine;

    public bool isAiming = false;


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

        if (!PickUpController.slotFull || PickUpController.weaponEquipped.gameObject.GetComponent<BaseWeapon>().isMelee) return;

        if (aimCoroutine != null) StopCoroutine(aimCoroutine);

        if (isAiming)
        {
            isAiming = false;
            aimCoroutine = StartCoroutine(StopAim());
        }
        else
        {
            isAiming = true;
            aimCoroutine = StartCoroutine(StartAim());
        }

    }

    public IEnumerator StartAim()
    {

        marker.SetActive(false);
        CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(40, 0.2f));
        gunSway.enabled = false;
        Vector3 targetPosition = PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().aimPosition;

        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier /= 2;
        gameObject.GetComponent<PlayerRotate>()._speed /= 2;

        // Instantly set rotation
        itemHolder.transform.localRotation = Quaternion.Euler(0, 90, 0);

        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.1f);
            yield return null;
        }

        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set

        aimCoroutine = null;
    }

    public IEnumerator StopAim()
    {
        marker.SetActive(true);
        Vector3 targetPosition = new Vector3(0.25f, -0.2f, 0.4f);
        CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(70, 0.1f));
        gameObject.GetComponent<PlayerRotate>()._speed *= 2;


        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier *= 2;


        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.1f);
            yield return null;
        }

        isAiming = false;
        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set
        gunSway.enabled = true;

        aimCoroutine = null;
    }
    
    public void ResetAim()
    {

        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            StopCoroutine(CameraEffects.Instance.ChangeFOVCoroutine);
        }

        gameObject.GetComponent<PlayerRotate>()._speed *= 2;
        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier *= 2;

        isAiming = false;
        StartCoroutine(CameraEffects.Instance.ChangeFOV(70, 0.1f));
        marker.SetActive(true);
        itemHolder.transform.localPosition = new Vector3(0.25f, -0.2f, 0.4f);
        gunSway.enabled = true;
    }


}
