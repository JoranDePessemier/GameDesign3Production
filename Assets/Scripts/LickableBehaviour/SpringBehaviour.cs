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

    private void Awake()
    {
        _transform = transform;
    }

    private void OnTriggerEnter(Collider other)
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
