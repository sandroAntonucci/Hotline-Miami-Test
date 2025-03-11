using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private float _rotationLimit;
    public float _speed;

    [SerializeField] private InputActionAsset _playerControls;

    private InputAction lookAction;

    protected float vertRot;
    protected Vector2 lookInput;

    private void Awake()
    {
        lookAction = _playerControls.FindActionMap("Player").FindAction("Look");

        lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        lookAction.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    public virtual void Rotate()
    {
        vertRot -= GetVerticalValue();
        vertRot = Mathf.Clamp(vertRot, -_rotationLimit, _rotationLimit);

        RotateVertical();
        RotateHorizontal();

    }

    protected float GetVerticalValue()
    {
        return lookInput.y * _speed * Time.deltaTime;
    }

    protected float GetHorizontalValue()
    {
        return lookInput.x * _speed * Time.deltaTime;
    }

    protected virtual void RotateVertical()
    {
        _cameraHolder.localRotation = Quaternion.Euler(vertRot, 0, 0);
    }

    protected virtual void RotateHorizontal()
    {
        transform.Rotate(Vector3.up * GetHorizontalValue());
    }





}
