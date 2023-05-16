using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Rigidbody _body;

    private Vector3 _previousFramePosition;

    private Vector3 _currentFramePosition;

    public Vector3 MovementOffSet { get; private set; }


    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _currentFramePosition = _body.position;
        MovementOffSet = _currentFramePosition - _previousFramePosition;
        _previousFramePosition = _currentFramePosition;
    }
}
