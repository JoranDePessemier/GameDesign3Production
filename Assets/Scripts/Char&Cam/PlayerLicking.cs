using System;
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

    [SerializeField]
    private LayerMask _nonLickableLayer;





    private Controls _controls;
    private Collider _tongueCollider;
    private Transform _transform;


    private Vector3 _startingPosition;

    private TongueState _state = TongueState.Holding;

    public ILickable HoldingObject { get; private set; }

    private bool _pickedUpObjectDuringPress;

    public event EventHandler<EventArgs> StartedHolding;

    public event EventHandler<EventArgs> StoppedHolding;

    private Renderer _renderer;



    private void Awake()
    {
        _transform = this.transform;
        _controls = new Controls();
        _tongueCollider = _transform.GetComponent<Collider>();
        _tongueCollider.enabled = false;



        _startingPosition = _transform.localPosition;

        _renderer = this.GetComponent<Renderer>();

    }


    private void TongueReleased(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (HoldingObject != null && !_pickedUpObjectDuringPress && HoldingObject.LickedReleased(_playerTransform))
        {
            HoldingObject = null;
            OnStoppedHolding(EventArgs.Empty);
        }

        _pickedUpObjectDuringPress = false;
    }


    private void TonguePressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_state == TongueState.Holding && HoldingObject == null)
        {
            _state = TongueState.OutGoing;
        }
    }



    private void Update()
    {

        if (HoldingObject != null && !_pickedUpObjectDuringPress && _controls.PlayerControls.TongueHolding.inProgress)
        {
            HoldingObject.HoldingLicked(_playerTransform);
        }


        float zScale = _transform.localScale.z;

        switch (_state)
        {
            case TongueState.Holding:

                _tongueCollider.enabled = false;
                _transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, 0);
                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _startingPosition.z);

                _renderer.enabled = false;

                break;


            case TongueState.OutGoing:

                _tongueCollider.enabled = true;

                zScale = Mathf.MoveTowards(zScale, _tongueMaxScale, _tongueMoveSpeed * Time.deltaTime);
                _transform.localScale = new Vector3(_transform.localScale.x, _transform.localScale.y, zScale);

                _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _startingPosition.z + zScale / 2);

                if (zScale >= _tongueMaxScale || HoldingObject != null)
                {
                    _state = TongueState.InGoing;
                }

                _renderer.enabled = true;

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

                _renderer.enabled = true;

                break;

        }
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.PlayerControls.TonguePressed.performed += TonguePressed;
        _controls.PlayerControls.TongueReleased.performed += TongueReleased;
    }

    private void OnDisable()
    {
        _controls.Enable();
        _controls.PlayerControls.TonguePressed.performed -= TonguePressed;
        _controls.PlayerControls.TongueReleased.performed -= TongueReleased;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if ((_lickableLayer & (1 << collisionObject.layer)) != 0 && HoldingObject == null)
        {
            
            if (!collisionObject.TryGetComponent<ILickable>(out ILickable lickCollision))
            {
                Debug.LogWarning($"{collisionObject} is in the lickable layer and does not have a lickable class attached");
            }
            

            lickCollision.Licked(_playerTransform);

            if (!lickCollision.IsEatable)
            {
                HoldingObject = null;
                _state = TongueState.InGoing;
            }
            else
            {
                HoldingObject = lickCollision;
                OnStartedHolding(EventArgs.Empty);
            }

            if (_controls.PlayerControls.TongueHolding.inProgress)
            {
                _pickedUpObjectDuringPress = true;
            }

        }
        else if((_nonLickableLayer & (1 << collisionObject.layer)) != 0)
        {
            _state = TongueState.InGoing;
        }
    }

    private void OnStartedHolding(EventArgs eventArgs)
    {
        var handler = StartedHolding;
        handler?.Invoke(this, eventArgs);
    }

    private void OnStoppedHolding(EventArgs eventArgs)
    {
        var handler = StoppedHolding;
        handler?.Invoke(this, eventArgs);
    }

    internal void RemoveCurrentHoldingObject()
    {
        HoldingObject.AttachedObject.SetActive(false);
        HoldingObject = null;
        OnStoppedHolding(EventArgs.Empty);
    }
}
