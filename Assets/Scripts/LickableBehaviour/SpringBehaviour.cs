using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpringBehaviour : MonoBehaviour
{
    [SerializeField]
    private LayerMask _springObjects;

    [SerializeField]
    private LayerMask _player;

    [SerializeField]
    private float _playerJumpForce;

    [SerializeField]
    private float _rigidbodyJumpForce;

    [SerializeField]
    private UnityEvent _activated;

    private Transform _transform;

    private bool _canSpring = true;

    private void Awake()
    {
        _transform = transform;
        if(TryGetComponent<LickEatable>(out LickEatable lickEatable))
        {
            lickEatable.Eaten += LickEatable_Eaten;
            lickEatable.Released += LickEatable_Released;
        }
    }

    private void LickEatable_Eaten(object sender, EventArgs e)
    {
        _canSpring = false;
    }

    private void LickEatable_Released(object sender, EventArgs e)
    {
        _canSpring = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (_canSpring)
        {
            GameObject collisionObject = other.gameObject;
            Rigidbody body = collisionObject.GetComponent<Rigidbody>();


            if ((_player & (1 << collisionObject.layer)) != 0)
            {
                CharacterMovement characterMovement = collisionObject.GetComponent<CharacterMovement>();

                characterMovement.Velocity = Vector3.zero;
                characterMovement.AddForce(_playerJumpForce * _transform.up);
                _activated?.Invoke();
            }
            else if ((_springObjects & (1 << collisionObject.layer)) != 0 && body.position.y > _transform.position.y)
            {
                body.velocity = Vector3.zero;
                body.velocity = _rigidbodyJumpForce * _transform.up;

                //body.AddForce(,ForceMode.VelocityChange);

                _activated?.Invoke();
            }
        }

    }

  
}
