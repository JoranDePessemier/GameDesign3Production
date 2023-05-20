using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour, ILickable
{
    public bool IsEatable => false;

    public GameObject AttachedObject => this.gameObject;

    [SerializeField]
    private Vector3 _spawnPointOffset;

    [SerializeField]
    private UnityEvent _activated;

    [SerializeField]
    private UnityEvent _deactivated;

    private Vector3 _spawnPointPosition;

    private void Awake()
    {
        _spawnPointPosition = this.transform.position;
    }

    public void HoldingLicked(Transform playerTransform)
    {
    }

    public void Licked(Transform playerTransform)
    {
        RespawnTracker.Instance.SetSpawn(this, _spawnPointPosition + _spawnPointOffset);
    }

    public bool LickedReleased(Transform playerTransform)
    {
        return true;
    }

    internal void SetAsActive()
    {
        _activated.Invoke();
    }

    internal void SetAsInActive()
    {
        _activated.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position,transform.position + _spawnPointOffset);
    }
}
