using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverBehaviour : MonoBehaviour, ILickable
{
    [SerializeField]
    private List<Activatable> _activatableObjects = new List<Activatable>();

    private bool _isActivated = false;

    bool ILickable.IsEatable => true;
    GameObject ILickable.AttachedObject { get { return this.gameObject; } }

    [SerializeField]
    private UnityEvent Activated;

    [SerializeField]
    private UnityEvent DeActivated;

    private void Awake()
    {
    
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
            foreach (Activatable activatable in _activatableObjects)
            {
                    activatable.Activate();        
            }
        }
        else
        {
            DeActivated?.Invoke();
            foreach (Activatable activatable in _activatableObjects)
            {
                activatable.Deactivate();
            }
        }

        
    }

    public bool LickedReleased(Transform playerTransform)
    {
        return true;
    }


}
