using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBase : Activatable
{

    [SerializeField]
    protected Transform[] _movingPoints;

    [SerializeField]
    protected float _movingSpeed;

    [SerializeField]
    protected Rigidbody _platformBody;

    protected int _currentMovingPointIndex;

    protected bool _activated;

    private void Awake()
    {
        _currentMovingPointIndex = 0;
        _platformBody.position = _movingPoints[_currentMovingPointIndex].position;
    }

    public override void Activate()
    {
        _activated = true;
    }

    protected virtual void FixedUpdate()
    {
        if (_activated)
        {
            _platformBody.MovePosition(Vector3.MoveTowards(_platformBody.position, _movingPoints[_currentMovingPointIndex].position, _movingSpeed * Time.deltaTime));
        }
        else
        {
            DeactivatedMovement();
        }

        if(_platformBody.position.Equals(_movingPoints[_currentMovingPointIndex].position))
        {
            ChangeMovingPoint();
        }
    }

    protected virtual void DeactivatedMovement()
    {
    }

    protected virtual void ChangeMovingPoint()
    {
    }

    public override void Deactivate()
    {
        _activated = false;
    }
}
