using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField]
    private UnityEvent _activated;

    [SerializeField]
    private UnityEvent _deactivated;

    [SerializeField]
    private Transform _spawnPosition;

    [SerializeField]
    private LayerMask _playerLayer;



    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if((_playerLayer & (1 << collisionObject.layer)) != 0)
        {
            RespawnTracker.Instance.SetSpawn(this, _spawnPosition);
        }
    }


    internal void SetAsActive()
    {
        _activated.Invoke();
    }

    internal void SetAsInActive()
    {
        _deactivated.Invoke();
    }

}
