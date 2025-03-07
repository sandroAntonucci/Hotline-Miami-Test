using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateSmooth : PlayerRotate
{

    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private Transform _horiRotHelper;

    private float _vertOld;
    private float _vertAngularVelocity;
    private float _horiAngularVelocity;

    private void Start()
    {
        _horiRotHelper.localRotation = transform.localRotation;
    }

    public override void Rotate()
    {
        _vertOld = vertRot;
        base.Rotate();
    }

    protected override void RotateHorizontal()
    {
        _horiRotHelper.Rotate(Vector3.up * GetHorizontalValue(), Space.Self);
        transform.localRotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.localRotation.eulerAngles.y, _horiRotHelper.localRotation.eulerAngles.y, ref _horiAngularVelocity, _smoothTime), 0);
    }

    protected override void RotateVertical()
    {
        vertRot = Mathf.SmoothDampAngle(_vertOld, vertRot, ref _vertAngularVelocity, _smoothTime);
        base.RotateVertical();
    }

}
