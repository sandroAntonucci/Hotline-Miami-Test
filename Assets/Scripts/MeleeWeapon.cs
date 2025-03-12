using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : BaseWeapon
{

    [SerializeField] private InputActionAsset PlayerControls;
    private InputAction attackAction;

    public Animator attackAnimation;

    private void Awake()
    {

        attackAction = PlayerControls.FindAction("Shoot");
        attackAction.performed += _ =>
        {
            Attack();
        };

        attackAnimation = GetComponent<Animator>();


    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void Attack()
    {
        attackAnimation.enabled = true;
    }



}
