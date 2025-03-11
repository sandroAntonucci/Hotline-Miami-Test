using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeStick : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<PickUpController>().equipped)
        {

            // Gets stuck
            rb.useGravity = false;
            rb.isKinematic = true;



        }
    }
}
