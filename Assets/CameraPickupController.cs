using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableController : MonoBehaviour
{

    private InteractableItem currentItem;

    private void Update()
    {

        Ray ray = new Ray(transform.position + transform.forward * 0.1f, transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {

            InteractableItem item = hit.collider.GetComponent<InteractableItem>();

            if (item != null && Vector3.Distance(transform.position, hit.collider.transform.position) < item.interactionRange && currentItem != item)
            {
                
                item.isInteracting = true;
                currentItem = item;
                currentItem.ChangeInteractionText();
            }
            else if (item == null || currentItem != item || Vector3.Distance(transform.position, hit.collider.transform.position) > item.interactionRange)
            {
                if (currentItem != null)
                {
                    ResetItem();
                }
            }
        }

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
        currentItem.isInteracting = false;
        currentItem = null;

        foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
        {
            gO.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

}
