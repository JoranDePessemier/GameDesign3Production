using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _charCtrl;
    private Controls _controls;

    private Transform _transform;


    [SerializeField]
    private float _acceleration = 5f;
    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30 km/h

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    [Tooltip("Drag can be used to slow down an object. The higher the drag the more the object slows down.")]
    private float _dragOnGround = 0f;

    [SerializeField]
    [Tooltip("The force applied when jumping")]
    private float _jumpForce = 2;

    [SerializeField]
    private float _gravityMultiplierHoldingJump;

    [SerializeField]
    private float _gravityMultiplierJumpRelease;

    private bool _jump;
    private bool _isHoldingJump;


    private Vector3 _velocity;
    private Vector3 _inputVector;


    private void Awake()
    {
        _charCtrl = this.GetComponent<CharacterController>();

        _controls = new Controls();
        _controls.Enable();
        _controls.PlayerControls.Jump.performed += Jump;

        _transform = this.transform;

        
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _jump = true;
    }

    void Update()
    {
        _inputVector.x = _controls.PlayerControls.Movement.ReadValue<Vector2>().x;
        _inputVector.z = _controls.PlayerControls.Movement.ReadValue<Vector2>().y;

        _isHoldingJump = _controls.PlayerControls.Jump.IsPressed();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyMovement();
        ApplyGroundDrag();
        ApplySpeedLimitation();
        ApplyJump();
        _charCtrl.Move(_velocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {

        _velocity += _transform.forward * _inputVector.magnitude * _acceleration;

    }

    private void ApplyGroundDrag()
    {
        if (_charCtrl.isGrounded)
        {
            _velocity *= (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplySpeedLimitation()
    {
        float tempY = _velocity.y;

        _velocity.y = 0;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxRunningSpeed);

        _velocity.y = tempY;
    }

    private void ApplyGravity()
    {
        if (_charCtrl.isGrounded)
        {
            //_velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
            _velocity.y = Physics.gravity.y * _charCtrl.skinWidth;
        }
        else
        {
            // g[m/s^2] * t[s]

            if (_isHoldingJump)
            {
                _velocity.y += Physics.gravity.y * Time.deltaTime * _gravityMultiplierHoldingJump;
            }
            else
            {
                _velocity.y += Physics.gravity.y * Time.deltaTime * _gravityMultiplierJumpRelease;
            }
           
        }
    }

    private void ApplyJump()
    {
        if (_jump && _charCtrl.isGrounded)
        {
            // We add the jumpforce, calculated in the Start function
            _velocity.y += _jumpForce;
        }
        _jump = false;
    }

    public void TargetRotation(Vector3 target)
    {
        _transform.forward = Vector3.Slerp(_transform.forward, target, Time.deltaTime * _rotationSpeed);
    }
}
