using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float _maxAirSpeed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    [Tooltip("Drag can be used to slow down an object. The higher the drag the more the object slows down.")]
    
    private float _dragOnGround = 0f;

    [SerializeField]
    private float _dragInAir;

    [SerializeField]
    [Tooltip("The force applied when jumping")]
    private float _jumpForce = 2;

    [SerializeField]
    private float _gravityMultiplierHoldingJump;

    [SerializeField]
    private float _gravityMultiplierJumpRelease;

    [SerializeField]
    private float _coyoteTime;

    [Header("Moving Offset Calculation")]
    [SerializeField]
    Vector3 _sphereCastPosition;

    [SerializeField]
    float _sphereCastRadius;

    public bool CanPerformControllerMove { get; set; } = true;
    public bool CanJump { get; set; } = true;

    private bool _canMove = true;

    Vector3 _movementOffset;

    private bool _jump;
    private bool _isJumping;
    private bool _isHoldingJump;

    private bool _previousGrounded;
    private bool _triggersPressed;
    private bool _previousTriggersPressed;


    private Vector3 _velocity;

    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }


    private Vector3 _inputVector;

    private Vector3 _previousInputVector;

    private bool _coyote;


    private void Awake()
    {
        _charCtrl = this.GetComponent<CharacterController>();

        _controls = new Controls();
        _controls.Enable();
        _controls.PlayerControls.JumpPressed.performed += Jump;

        _transform = this.transform;

        _previousGrounded = _charCtrl.isGrounded;
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CanJump)
        {
            _jump = true;
            
        }

    }

    void Update()
    {
        _inputVector.x = _controls.PlayerControls.Movement.ReadValue<Vector2>().x;
        _inputVector.z = _controls.PlayerControls.Movement.ReadValue<Vector2>().y;
        _triggersPressed = _controls.PlayerControls.StandStill.IsPressed();

        if((_inputVector != Vector3.zero && _previousInputVector == Vector3.zero && _charCtrl.isGrounded) || (_charCtrl.isGrounded && !_previousGrounded && _inputVector != Vector3.zero) || (!_triggersPressed && _previousTriggersPressed && _inputVector != Vector3.zero))
        {
            GlobalAudioManager.Instance?.PlaySound("Running");
        }
        else if(_inputVector == Vector3.zero && _previousInputVector != Vector3.zero || !_charCtrl.isGrounded || _triggersPressed)
        {
            GlobalAudioManager.Instance?.StopSound("Running");
        }

        _isHoldingJump = _controls.PlayerControls.JumpHolding.IsPressed();

        _canMove = !_controls.PlayerControls.StandStill.IsPressed();


        if (!_charCtrl.isGrounded && _previousGrounded && !_isJumping)
        {
            StartCoroutine(CountCoyote());
        }

        _previousInputVector = _inputVector;
        _previousGrounded = _charCtrl.isGrounded;
        _previousTriggersPressed = _triggersPressed;

    }

    private IEnumerator CountCoyote()
    {
        _coyote = true;
        yield return new WaitForSeconds(_coyoteTime);
        _coyote = false;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyMovement();
        ApplyDrag();
        ApplySpeedLimitation();
        ApplyJump();
        CalculateMovingObjectOffset();
        if (CanPerformControllerMove)
        {
            _charCtrl.Move(_velocity * Time.deltaTime + _movementOffset);
        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + _sphereCastPosition, _sphereCastRadius);
    }

    private void CalculateMovingObjectOffset()
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position + _sphereCastPosition, _sphereCastRadius);
        List<MovingObject> standingObjects = new List<MovingObject>();

        foreach (Collider collider in colliders)
        {
            if(collider.transform.TryGetComponent<MovingObject>(out MovingObject moving))
            {
                standingObjects.Add(moving);
            }
        }

        _movementOffset = Vector3.zero;

        foreach (var standingObject in standingObjects)
        {
            _movementOffset += standingObject.MovementOffSet;
        }

    }

    private void ApplyMovement()
    {
        if(_canMove)
        {
            _velocity += _transform.forward * _inputVector.magnitude * _acceleration;
        }


    }

    private void ApplyDrag()
    {
        float drag = 1;

        if (_charCtrl.isGrounded )
        {
             drag = (1 - Time.deltaTime * _dragOnGround);
        }
        else
        {
            drag = (1 - Time.deltaTime * _dragInAir);
        }

        _velocity.x *= drag;
        _velocity.z *= drag;
    }

    private void ApplySpeedLimitation()
    {
        float tempY = _velocity.y;

        _velocity.y = 0;
        if(_charCtrl.isGrounded )
        {
            _velocity = Vector3.ClampMagnitude(_velocity, _maxRunningSpeed);
        }
        else
        {
            _velocity = Vector3.ClampMagnitude(_velocity, _maxAirSpeed);
        }


        _velocity.y = tempY;
    }

    private void ApplyGravity()
    {
        if (_charCtrl.isGrounded && _velocity.y <= 0)
        {
            //_velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
            _velocity.y = Physics.gravity.y * _charCtrl.skinWidth;
            _isJumping = false;
        }
        else if (_isJumping)
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
        else
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
       
    }

    private void ApplyJump()
    {
        if (_jump && (_charCtrl.isGrounded || _coyote))
        {
            // We add the jumpforce, calculated in the Start function
            _velocity.y = 0;
            _velocity.y += _jumpForce;
            GlobalAudioManager.Instance?.PlaySound("Jump");
            _isJumping = true;
        }
        _jump = false;
    }

    public void AddForce(Vector3 force)
    {
        _velocity += force;
    }

    public void TargetRotation(Vector3 target)
    {
        _transform.forward = Vector3.Slerp(_transform.forward, target, Time.deltaTime * _rotationSpeed);
    }
}
