using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour, IActivator
{
    [SerializeField]
    private List<Activatable> _activatableObjects = new List<Activatable>();

    public bool Activated { get; private set; }

    private bool _previousFrameActivated;

    public List<Activatable> ActivatableObjects
    {
        get { return _activatableObjects; }
        set { _activatableObjects = value; }
    }

    [SerializeField]
    private LayerMask _activatingLayer;

    [SerializeField]
    private UnityEvent EventActivated;

    [SerializeField]
    private UnityEvent EventDeActivated;

    private bool _canActivate =true;


    private void Start()
    {
        SendPresenceToActivatables();

        if(TryGetComponent<LickEatable>(out LickEatable lickEatable))
        {
            lickEatable.Eaten += LickEatable_Eaten;
            lickEatable.Released += LickEatable_Released;
        }
    }

    private void LickEatable_Released(object sender, EventArgs e)
    {

        _canActivate = true;
    }

    private void LickEatable_Eaten(object sender, EventArgs e)
    {
        _canActivate = false;
    }

    private void FixedUpdate()
    {
        Activated = false;
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if(((_activatingLayer & (1 << collisionObject.layer)) != 0) && collisionObject.GetComponent<Collider>().enabled && _canActivate)
        {
            Activated = true;
        }
    }

    private void LateUpdate()
    {
        if(Activated && !_previousFrameActivated)
        {
            Activate();
        }
        else if(!Activated && _previousFrameActivated)
        {
            DeActivate();
        }

        _previousFrameActivated = Activated;
    }


    private void Activate()
    {
        EventActivated?.Invoke();

        foreach (Activatable activatable in ActivatableObjects)
        {
            bool shouldActivate = true;
            foreach (IActivator activator in activatable.Activators)
            {
                if (activator.GetType() == typeof(ButtonBehaviour))
                {
                    ButtonBehaviour button = activator.ConvertTo<ButtonBehaviour>();

                    if(button != this && button.Activated)
                    {
                        shouldActivate = false;
                    }


                }
            }

            if (shouldActivate)
            {
                activatable.Activate();
            }
        }
    }

    private void DeActivate()
    {
        EventDeActivated?.Invoke();

        foreach (Activatable activatable in ActivatableObjects)
        {
            bool shouldDeActivate = true;
            foreach (IActivator activator in activatable.Activators)
            {
                if(activator.GetType() == typeof(ButtonBehaviour) && activator.ConvertTo<ButtonBehaviour>().Activated)
                {
                    shouldDeActivate = false;
                }
            }

            if (shouldDeActivate)
            {
                activatable.Deactivate();
            }

        }
    }

    public void SendPresenceToActivatables()
    {
        foreach (Activatable activatable in ActivatableObjects)
        {
            activatable.Activators.Add(this);
        }
    }
}
