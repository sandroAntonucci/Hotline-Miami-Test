using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{

    private PickUpController gunToPick;

    private void Update()
    {
        // Draws raycast from the camera to know where the player is looking
        Transform cameraHolder = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Ray ray = new Ray(cameraHolder.position + cameraHolder.forward * 0.1f, cameraHolder.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.CompareTag("Gun") && Vector3.Distance(transform.position, hit.collider.transform.position) < hit.collider.GetComponent<PickUpController>().pickUpRange && !hit.collider.GetComponent<PickUpController>().equipped)
            {

                if(gunToPick != null)
                {
                    gunToPick.playerCanPick = false;
                    gunToPick = null;
                }

                gunToPick = hit.collider.GetComponent<PickUpController>();

                foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
                {
                    gO.GetComponent<TextMeshProUGUI>().text = "[E] TO PICK UP";
                }

                gunToPick.playerCanPick = true;
            }
            else
            {
                foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
                {
                    gO.GetComponent<TextMeshProUGUI>().text = "";
                }

                if (gunToPick != null)
                {
                    gunToPick.playerCanPick = false;
                    gunToPick = null;
                }

            }

        }

        else
        {
            foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
            {
                gO.GetComponent<TextMeshProUGUI>().text = "";
            }
        }


    }



}
