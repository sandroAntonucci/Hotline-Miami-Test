using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{

    public string interactionText;

    public bool isInteracting = false;

    public float interactionRange;


    public virtual void ChangeInteractionText()
    {

        foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
        {
            gO.GetComponent<TMPro.TextMeshProUGUI>().text = interactionText;
        }

    }
}
