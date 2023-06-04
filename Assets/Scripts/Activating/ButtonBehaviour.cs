using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

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

    [Header("LineRenderer")]
    [SerializeField]
    LineRenderer _lineRenderer;

    private List<LineRenderer> _lines = new List<LineRenderer>();

    private Controls _controls;

    private bool _canActivate =true;
    private bool _linesShouldUpdate;

    private Transform _transform;

    private void OnEnable()
    {
        _controls.Enable();
        _controls.PlayerControls.SeeConnectionsPressed.performed += ShowConnections;
        _controls.PlayerControls.SeeConnectionsReleased.performed += HideConnections;
    }

    private void HideConnections(InputAction.CallbackContext obj)
    {
        foreach(LineRenderer line in _lines)
        {
            line.enabled = false;
        }
        _linesShouldUpdate = false;
    }

    private void ShowConnections(InputAction.CallbackContext obj)
    {
        foreach (LineRenderer line in _lines)
        {
            line.enabled = true;
        }
        _linesShouldUpdate=true;
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.PlayerControls.SeeConnectionsPressed.performed -= ShowConnections;
        _controls.PlayerControls.SeeConnectionsReleased.performed -= HideConnections;
    }

    private void Awake()
    {
        _transform = transform;
        _controls = new Controls();
    }


    private void Start()
    {
        SendPresenceToActivatables();
        CreateLineRenderers();

        if(TryGetComponent<LickEatable>(out LickEatable lickEatable))
        {
            lickEatable.Eaten += LickEatable_Eaten;
            lickEatable.Released += LickEatable_Released;
        }
    }

    private void CreateLineRenderers()
    {
        foreach(Activatable activatable in _activatableObjects)
        {
            LineRenderer lineRenderer = GameObject.Instantiate(_lineRenderer.gameObject,_transform).GetComponent<LineRenderer>();

            lineRenderer.SetPosition(0, _transform.position);
            lineRenderer.SetPosition(1,activatable.GetTransform().position);

            _lines.Add(lineRenderer);
            lineRenderer.enabled = false;
        }
    }

    private void LickEatable_Released(object sender, EventArgs e)
    {

        _canActivate = true;
    }

    private void LickEatable_Eaten(object sender, EventArgs e)
    {
        DeActivate();
        _canActivate = false;
    }

    private void FixedUpdate()
    {
        Activated = false;
    }

    private void Update()
    {
        if(_linesShouldUpdate)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                LineRenderer lineRenderer = _lines[i];
                lineRenderer.SetPosition(0, _transform.position);
                lineRenderer.SetPosition(1, _activatableObjects[i].GetTransform().position);
            }
        }

        
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        Collider collider = collisionObject.GetComponent<Collider>();

        if(((_activatingLayer & (1 << collisionObject.layer)) != 0) && collider.enabled && _canActivate)
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
