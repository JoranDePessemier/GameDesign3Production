using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableTest : Activatable
{
    [SerializeField]
    private Material _activeMaterial;

    [SerializeField]
    private Material _inactiveMaterial;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _inactiveMaterial;
    }

    public override void Activate()
    {
       _renderer.material = _activeMaterial;
    }

    public override void Deactivate()
    {
        _renderer.material = _inactiveMaterial;
    }
}
