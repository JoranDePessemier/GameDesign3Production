using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement _cMovement;

    [SerializeField]
    private Vector2 _rotateSpeed;

    [SerializeField]
    private Transform _cameraY;

    private Transform _charTransform, _transform;
    private Controls _controls;
    private float _currentYRotation;
    private bool _canRotate = true;

    private void Awake()
    {
        _charTransform = _cMovement.transform;
        _transform = this.transform;

        _controls = new Controls();
        _controls.Enable();
    }

    private void Update()
    {
        _transform.position = _charTransform.position;

        float stickX = _controls.PlayerControls.CameraMovement.ReadValue<Vector2>().x;
        float stickY = _controls.PlayerControls.CameraMovement.ReadValue<Vector2>().y;

        Vector2 playerMovement = _controls.PlayerControls.Movement.ReadValue<Vector2>();

        float targetRotation = Mathf.Atan2(_transform.forward.z, _transform.forward.x) + Mathf.Atan2(playerMovement.y, playerMovement.x) - Mathf.PI / 2;

        if (!playerMovement.sqrMagnitude.Equals(0) && _canRotate)
        {
            _cMovement.TargetRotation(new Vector3(Mathf.Cos(targetRotation), 0, Mathf.Sin(targetRotation)));
        }



        _currentYRotation -= stickY * Time.deltaTime * _rotateSpeed.y;
        _currentYRotation = Mathf.Clamp(_currentYRotation, -90, 90);

        _transform.Rotate(0, stickX * Time.deltaTime * _rotateSpeed.x, 0);
        _cameraY.eulerAngles = new Vector3(_currentYRotation, _cameraY.eulerAngles.y, _cameraY.eulerAngles.z);
    }
}
