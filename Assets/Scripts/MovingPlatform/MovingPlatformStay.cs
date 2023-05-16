using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStay : MovingPlatformBase
{

    bool _previousFrameActivated;

    protected override void ChangeMovingPoint()
    {
        base.ChangeMovingPoint();
        int nextIndex = 0;

        if(_activated) 
        {
            nextIndex = _currentMovingPointIndex + 1;

            if(nextIndex >= _movingPoints.Length)
            {
                nextIndex = _currentMovingPointIndex;
            }
        }
        else
        {
            nextIndex = _currentMovingPointIndex - 1;

            if (nextIndex < 0)
            {
                nextIndex = _currentMovingPointIndex;
            }
        }

        _currentMovingPointIndex = nextIndex;
    }

    protected override void FixedUpdate()
    {
        if (_activated)
        {
            _platformBody.MovePosition(Vector3.MoveTowards(_platformBody.position, _movingPoints[_currentMovingPointIndex].position, _movingSpeed * Time.deltaTime));
        }
        else
        {
            DeactivatedMovement();
        }

        if (_platformBody.position.Equals(_movingPoints[_currentMovingPointIndex].position) || _activated != _previousFrameActivated )
        {
            ChangeMovingPoint();
        }

        _previousFrameActivated = _activated;

    }

    protected override void DeactivatedMovement()
    {
        base.DeactivatedMovement();

        _platformBody.MovePosition(Vector3.MoveTowards(_platformBody.position, _movingPoints[_currentMovingPointIndex].position, _movingSpeed * Time.deltaTime));
    }
}
