using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    private void Start()
    {
       SendPresenceToActivatables();
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
}
