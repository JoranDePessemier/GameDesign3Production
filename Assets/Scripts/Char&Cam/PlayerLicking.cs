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
    private Transform _tongueTransform;

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
        _controls = new Controls();
        _tongueCollider = _tongueTransform.GetComponent<Collider>();
        _tongueCollider.enabled = false;

        _controls.PlayerControls.TonguePressed.performed += TonguePressed;
        _controls.PlayerControls.TongueReleased.performed += TongueReleased;

        _startingPosition = _tongueTransform.localPosition;

        _transform = this.transform;
    }

    private void TongueReleased(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_holdingObject != null && !_pickedUpObjectDuringPress && _holdingObject.LickedReleased(_transform))
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
            _holdingObject.HoldingLicked(_transform);
        }


        float zScale = _tongueTransform.localScale.z;

        switch (_state)
        {
            case TongueState.Holding:

                _tongueCollider.enabled = false;
                _tongueTransform.localScale = new Vector3(_tongueTransform.localScale.x, _tongueTransform.localScale.y, 0);
                _tongueTransform.localPosition = new Vector3(_tongueTransform.localPosition.x, _tongueTransform.localPosition.y, _startingPosition.z);

                break;


            case TongueState.OutGoing:

                _tongueCollider.enabled = true;

                zScale = Mathf.MoveTowards(zScale, _tongueMaxScale, _tongueMoveSpeed * Time.deltaTime);
                _tongueTransform.localScale = new Vector3(_tongueTransform.localScale.x, _tongueTransform.localScale.y, zScale);

                _tongueTransform.localPosition = new Vector3(_tongueTransform.localPosition.x, _tongueTransform.localPosition.y, _startingPosition.z + zScale / 2);

                if (zScale >= _tongueMaxScale || _holdingObject != null)
                {
                    _state = TongueState.InGoing;
                }


                break;

            case TongueState.InGoing:

                _tongueCollider.enabled = false;


                zScale = Mathf.MoveTowards(zScale, 0, _tongueMoveSpeed * Time.deltaTime);
                _tongueTransform.localScale = new Vector3(_tongueTransform.localScale.x, _tongueTransform.localScale.y, zScale);

                _tongueTransform.localPosition = new Vector3(_tongueTransform.localPosition.x, _tongueTransform.localPosition.y, _startingPosition.z + zScale / 2);

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

            _holdingObject.Licked(_transform);

            if (_controls.PlayerControls.TongueHolding.inProgress)
            {
                _pickedUpObjectDuringPress = true;
            }

        }
    }
}
