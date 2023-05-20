using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    private PlayerLicking _lickingScript;

    [SerializeField]
    private GameObject _collisionGameObject;

    [SerializeField]
    private int _noPlayerCollisionLayer;

    [SerializeField]
    private int _playerCollisionLayer;

    void Start()
    {
        _lickingScript = FindObjectOfType<PlayerLicking>();
        _lickingScript.StartedHolding += Licking_StartedHolding;
        _lickingScript.StoppedHolding += Licking_StoppedHolding;

        _collisionGameObject.layer = _noPlayerCollisionLayer;
    }

    private void Licking_StoppedHolding(object sender, EventArgs e)
    {
        _collisionGameObject.layer = _noPlayerCollisionLayer;
    }

    private void Licking_StartedHolding(object sender, EventArgs e)
    {
        _collisionGameObject.layer = _playerCollisionLayer;
    }



}
