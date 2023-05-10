using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum TongueState
{
    Holding,
    OutGoing,
    InGoing
}

public class PlayerLicking : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;

    [SerializeField]
    private float _tongueMoveSpeed;

    [SerializeField]
    private float _tongueMaxScale;

    [SerializeField]
    private LayerMask _lickableLayer;





    private Controls _controls;
    private Collider _tongueCollider;
    private Transform _transform;


    private Vector3 _startingPosition;

    private TongueState _state = TongueState.Holding;

    private ILickable _holdingObject;

    private bool _pickedUpObjectDuringPress;



    private void Awake()
    {
        _transform = this.transform;
        _controls = new Controls();
        _tongueCollider = _transform.GetComponent<Collider>();
        _tongueCollider.enabled = false;

        _controls.PlayerControls.TonguePressed.performed += TonguePressed;
        _controls.PlayerControls.TongueReleased.performed += TongueReleased;

        _startingPosition = _transform.localPosition;

    }

    private void TongueReleased(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_holdingObject != null && !_pickedUpObjectDuringPress && _holdingObject.LickedReleased(_playerTransform))
        {
            _holdingObject = null;
        }

        _pickedUpObjectDuringPress = false;
    }


    private void TonguePressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_state == TongueState.Holding && _holdingObject == null)
        {
            _state = TongueState.OutGoing;
        }
    }



    private void Update()
    {

        if (_holdingObject != null && !_pickedUpObjectDuringPress && _controls.PlayerControls.TongueHolding.inProgress)
        {
            _holdingObject.HoldingLicked(_playerTransform);
        }


        float zScale = _transform.localScale.z;

        switch (_state)
        {
            case TongueState.Holding:

                _tongueCollider.enabled = false;
                _transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, 0);
                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _startingPosition.z);

                break;


            case TongueState.OutGoing:

                _tongueCollider.enabled = true;

                zScale = Mathf.MoveTowards(zScale, _tongueMaxScale, _tongueMoveSpeed * Time.deltaTime);
                _transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, zScale);

                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _startingPosition.z + zScale / 2);

                if (zScale >= _tongueMaxScale || _holdingObject != null)
                {
                    _state = TongueState.InGoing;
                }


                break;

            case TongueState.InGoing:

                _tongueCollider.enabled = false;


                zScale = Mathf.MoveTowards(zScale, 0, _tongueMoveSpeed * Time.deltaTime);
                _transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, zScale);

                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _startingPosition.z + zScale / 2);

                if (zScale <= 0)
                {
                    _state = TongueState.Holding;
                }

                break;

        }
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if ((_lickableLayer & (1 << collisionObject.layer)) != 0)
        {
            collisionObject.SetActive(false);

            if (!collisionObject.TryGetComponent<ILickable>(out _holdingObject))
            {
                Debug.LogWarning($"{collisionObject} is in the lickable layer and does not have a lickable class attached");
            }

            _holdingObject.Licked(_playerTransform);

            if (_controls.PlayerControls.TongueHolding.inProgress)
            {
                _pickedUpObjectDuringPress = true;
            }

        }
    }
}
