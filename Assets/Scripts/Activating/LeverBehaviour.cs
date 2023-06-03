using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LeverBehaviour : MonoBehaviour, ILickable, IActivator
{
    [SerializeField]
    private List<Activatable> _activatableObjects = new List<Activatable>();

    private bool _isActivated = false;

    bool ILickable.IsEatable => false;
    GameObject ILickable.AttachedObject { get { return this.gameObject; } }

    public List<Activatable> ActivatableObjects
    {
        get { return _activatableObjects; }
        set { _activatableObjects = value; }
    }


    [SerializeField]
    private UnityEvent Activated;

    [SerializeField]
    private UnityEvent DeActivated;

    [Header("LineRenderer")]
    [SerializeField]
    LineRenderer _lineRenderer;


    private List<LineRenderer> _lines = new List<LineRenderer>();

    private Transform _transform;

    private Controls _controls;
    private bool _linesShouldUpdate;

    private void Awake()
    {
        _transform = transform;
        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.PlayerControls.SeeConnectionsPressed.performed += ShowConnections;
        _controls.PlayerControls.SeeConnectionsReleased.performed += HideConnections;
    }
    private void OnDisable()
    {
        _controls.Disable();
        _controls.PlayerControls.SeeConnectionsPressed.performed -= ShowConnections;
        _controls.PlayerControls.SeeConnectionsReleased.performed -= HideConnections;
    }

    private void HideConnections(InputAction.CallbackContext obj)
    {
        foreach (LineRenderer line in _lines)
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
        _linesShouldUpdate = true;
    }


    private void Start()
    {
       SendPresenceToActivatables();
        CreateLineRenderers();
    }

    private void CreateLineRenderers()
    {
        foreach (Activatable activatable in _activatableObjects)
        {
            LineRenderer lineRenderer = GameObject.Instantiate(_lineRenderer.gameObject, _transform).GetComponent<LineRenderer>();

            lineRenderer.SetPosition(0, _transform.position);
            lineRenderer.SetPosition(1, activatable.GetTransform().position);

            _lines.Add(lineRenderer);
        }
    }

    public void HoldingLicked(Transform playerTransform)
    {
        
    }

    public void Licked(Transform playerTransform)
    {
        _isActivated = !_isActivated;

        if (_isActivated)
        {
            Activated?.Invoke();
            foreach (Activatable activatable in ActivatableObjects)
            {
                    activatable.Activate();        
            }
        }
        else
        {
            DeActivated?.Invoke();
            foreach (Activatable activatable in ActivatableObjects)
            {
                activatable.Deactivate();
            }
        }

        
    }

    public bool LickedReleased(Transform playerTransform)
    {
        return true;
    }

    public void SendPresenceToActivatables()
    {
        foreach (Activatable activatable in ActivatableObjects)
        {
            activatable.Activators.Add(this);
        }
    }

    private void Update()
    {
        if (_linesShouldUpdate)
        {

            for (int i = 0; i < _lines.Count; i++)
            {
                LineRenderer lineRenderer = _lines[i];
                lineRenderer.SetPosition(0, _transform.position);
                lineRenderer.SetPosition(1, _activatableObjects[i].GetTransform().position);
            }

        }

    }
}
