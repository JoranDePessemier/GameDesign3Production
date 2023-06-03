using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public List<IActivator> Activators { get;  set; } = new List<IActivator>();

    public abstract Transform GetTransform();

    public abstract void Activate();

    public abstract void Deactivate();
}
