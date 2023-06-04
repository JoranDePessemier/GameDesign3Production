using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask _triggerLayer;

    [SerializeField]
    private UnityEvent _triggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if ((_triggerLayer & (1 << collisionObject.layer)) != 0)
        {
            _triggerEntered.Invoke();
        }
    }

}
