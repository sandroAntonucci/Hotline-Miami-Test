using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableController : MonoBehaviour
{

    public InteractableItem currentItem;

    private void Update()
    {

        Ray ray = new Ray(transform.position + transform.forward * 0.1f, transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {

            InteractableItem item = hit.collider.GetComponent<InteractableItem>();
            
            // If Raycast hits an interactable object and it isn't current interactable object
            if (item != null && Vector3.Distance(transform.position, hit.collider.transform.position) < item.interactionRange && currentItem != item && !item.isInteracting)
            {
                if (currentItem != null) ResetItem();

                item.canInteract = true;
                currentItem = item;
                currentItem.ChangeInteractionText();
            }

            // If Raycast hits an interactable object but its out of interaction range
            else if ((item != null && Vector3.Distance(transform.position, hit.collider.transform.position) >= item.interactionRange || item == null) && currentItem != null)  
            {
                ResetItem();
            }


        }

        // If Raycast doesn't hit anything
        else
        {
            if (currentItem != null)
            {
                ResetItem();
            }
        }
    }

    private void ResetItem()
    {
        currentItem.canInteract = false;
        currentItem = null;

        foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
        {
            gO.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

}
