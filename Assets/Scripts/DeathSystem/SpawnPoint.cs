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

    public Transform SpawnPosition => _spawnPosition;

    [SerializeField]
    private LayerMask _playerLayer;

    public event EventHandler<EventArgs> CheckPointActivated;



    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if((_playerLayer & (1 << collisionObject.layer)) != 0)
        {
            RespawnTracker.Instance.SetSpawn(this, _spawnPosition);
            OnCheckPointActivated(EventArgs.Empty);
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

    private void OnCheckPointActivated(EventArgs eventArgs)
    {
        var handler = CheckPointActivated;
        handler?.Invoke(this, eventArgs);
    }

}
