using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if ((_player & (1 << collisionObject.layer)) != 0)
            {
                collisionObject.GetComponent<CharacterMovement>().AddForce(_playerJumpForce * _transform.up);
            }
            else if ((_springObjects & (1 << collisionObject.layer)) != 0)
            {
                Rigidbody body = collisionObject.GetComponent<Rigidbody>();

                body.velocity = _rigidbodyJumpForce * _transform.up;

                //body.AddForce(,ForceMode.VelocityChange);
            }
        }

    }
}
