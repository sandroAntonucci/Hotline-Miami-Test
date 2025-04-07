using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{

    public PickUpController pickUpController;

    public bool isMelee = false;

    public bool isEnemyWeapon = false;

    private void Awake()
    {
        pickUpController = GetComponent<PickUpController>(); 

        if (isEnemyWeapon)
        {
            pickUpController.enabled = false;
        }
    }

    public void DropWeapon()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(rb.transform.right * -1 * 10    , ForceMode.Impulse);
        rb.AddForce(rb.transform.up * 6 / 2, ForceMode.Impulse);
        rb.AddTorque(rb.transform.forward, ForceMode.Impulse);

        pickUpController.enabled = true;
    }

    public virtual void SetUpPlayerWeapon()
    {

    }

}
