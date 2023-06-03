using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKeepMoving : MovingPlatformBase
{
    [SerializeField]
    private bool _circularMovement;

    [SerializeField]
    private bool _startActivated;

    [SerializeField]
    private float _waitingTime;

    private bool _movingBack;

    protected override void Awake()
    {
        base.Awake();
        _activated = _startActivated;
    }

    protected override void ChangeMovingPoint()
    {
        _canChangePivotPoint = false;
        StartCoroutine(ChangePoint());

    }

    private IEnumerator ChangePoint()
    {
        yield return new WaitForSeconds(_waitingTime);

        
        
        int nextIndex = _currentMovingPointIndex + (_movingBack ? -1 : 1);

        if (nextIndex >= _movingPoints.Length)
        {
            if (_circularMovement)
            {
                nextIndex = 0;
            }
            else
            {
                _movingBack = true;
                nextIndex = _movingPoints.Length - 2;
            }
        }
        else if (nextIndex < 0)
        {
            _movingBack = false;
            nextIndex = 1;
        }

        _currentMovingPointIndex = nextIndex;

        _canChangePivotPoint = true;
    }
}
