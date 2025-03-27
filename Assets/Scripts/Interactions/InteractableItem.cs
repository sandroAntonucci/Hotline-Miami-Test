using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableItem : MonoBehaviour
{

    [SerializeField] private InputActionAsset playerInput;

    private InputAction interactAction;

    public string interactionText;

    public bool canInteract = false;

    public bool isInteracting = false;

    public float interactionRange;

    public virtual void Awake()
    {
        interactAction = playerInput.FindAction("PickUp");

        interactAction.performed += _ =>
        {
            if (canInteract && !isInteracting)
            {
                Interaction();
            }

            else if (isInteracting)
            {
                StopInteraction();
            }

        };
    }

    public virtual void OnEnable()
    {
        interactAction.Enable();
    }

    public virtual void OnDisable()
    {
        interactAction.Disable();
    }

    public virtual void ChangeInteractionText()
    {

        foreach (var gO in GameObject.FindGameObjectsWithTag("InteractionText"))
        {
            gO.GetComponent<TMPro.TextMeshProUGUI>().text = interactionText;
        }

    }

    public virtual void Interaction()
    {

    }

    public virtual void StopInteraction()
    {

    }

}
