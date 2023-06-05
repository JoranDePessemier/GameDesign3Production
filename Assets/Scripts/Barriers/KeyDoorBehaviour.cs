using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDoorBehaviour : MonoBehaviour, ICharacterHit
{
    private PlayerLicking _lickingScript;

    private bool _canOpenDoor = false;

    [SerializeField]
    private int _keyLayer;

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private GameObject _particles;

    private Transform _transform;


    void Awake()
    {
        _lickingScript = FindObjectOfType<PlayerLicking>();
        _transform = transform;

    }

    private void OnEnable()
    {
        _lickingScript.StartedHolding += Licking_StartedHolding;
        _lickingScript.StoppedHolding += Licking_StoppedHolding;
    }

    private void OnDisable()
    {
        _lickingScript.StartedHolding -= Licking_StartedHolding;
        _lickingScript.StoppedHolding -= Licking_StoppedHolding;
    }

    private void Licking_StoppedHolding(object sender, EventArgs e)
    {
        _canOpenDoor = false;
    }

    private void Licking_StartedHolding(object sender, EventArgs e)
    {

        if (_lickingScript.HoldingObject.AttachedObject.layer == _keyLayer)
        {
            _canOpenDoor = true;
        }
    }


    private void DoorOpen()
    {
        GameObject.Instantiate(_particles, _transform.position, _transform.rotation);
        this.gameObject.SetActive(false);
        _lickingScript.RemoveCurrentHoldingObject();
    }

    public void CharacterHit(GameObject character)
    {
        if ((_playerLayer & (1 << character.layer)) != 0 && _canOpenDoor)
        {

            DoorOpen();
        }
    }
}
