using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Activatable> _activatableObjects = new List<Activatable>();

    private bool _activated = false;

    private bool _previousFrameActivated;


    [SerializeField]
    private LayerMask _activatingLayer;

    [SerializeField]
    private UnityEvent Activated;

    [SerializeField]
    private UnityEvent DeActivated;


    private void FixedUpdate()
    {
        _activated = false;
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if(((_activatingLayer & (1 << collisionObject.layer)) != 0) && collisionObject.GetComponent<Collider>().enabled)
        {
            _activated = true;
        }
    }

    private void LateUpdate()
    {
        if(_activated && !_previousFrameActivated)
        {
            Activate();
        }
        else if(!_activated && _previousFrameActivated)
        {
            DeActivate();
        }

        _previousFrameActivated = _activated;
    }


    private void Activate()
    {
        Activated?.Invoke();

        foreach (Activatable activatable in _activatableObjects)
        {
           activatable.Activate();
        }
    }

    private void DeActivate()
    {
        DeActivated?.Invoke();

        foreach (Activatable activatable in _activatableObjects)
        {
            activatable.Deactivate();
        }
    }


}
