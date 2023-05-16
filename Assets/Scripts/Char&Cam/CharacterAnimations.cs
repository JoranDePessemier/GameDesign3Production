using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Vector2 _minMaxGroundVelocity;

    [SerializeField]
    private Vector2 _minMaxRunningSpeed;

    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        Vector2 groundMovement = new Vector3(_controller.velocity.x, _controller.velocity.z);
        float airMovement = _controller.velocity.y;

        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isJumping", false);
        _animator.SetBool("isFalling", false);

        _animator.speed = 1;

        if (_controller.isGrounded)
        {
            float movementSpeed = groundMovement.magnitude;

            if(movementSpeed != 0 && _controls.PlayerControls.Movement.ReadValue<Vector2>().SqrMagnitude() != 0)
            {
                _animator.SetBool("isWalking", true);
                _animator.speed = (movementSpeed - _minMaxGroundVelocity.x) / (_minMaxGroundVelocity.y - _minMaxGroundVelocity.x) * (_minMaxRunningSpeed.y - _minMaxRunningSpeed.x) + _minMaxRunningSpeed.x;
            }
            else
            {
                _animator.SetBool("isIdle", true);
            }
        }
        else
        {
            if(airMovement > 0)
            {
                _animator.SetBool("isJumping", true);
            }
            else
            {
                _animator.SetBool("isFalling", true);
            }
        }
    }
}
