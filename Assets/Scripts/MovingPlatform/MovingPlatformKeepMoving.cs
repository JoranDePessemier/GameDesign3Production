using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformKeepMoving : MovingPlatformBase
{
    [SerializeField]
    private bool _circularMovement;

    [SerializeField]
    private bool _startActivated;

    private bool _movingBack;

    protected override void Awake()
    {
        _activated = _startActivated;
    }

    protected override void ChangeMovingPoint()
    {
        int nextIndex = _currentMovingPointIndex + (_movingBack ? -1 : 1);
        
        if(nextIndex >= _movingPoints.Length)
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
        else if(nextIndex < 0)
        {
            _movingBack = false;
            nextIndex = 1;
        }

        _currentMovingPointIndex = nextIndex;
    }
}
