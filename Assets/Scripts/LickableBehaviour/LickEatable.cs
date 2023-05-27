using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LickEatable : MonoBehaviour, ILickable, IRespawn
{
    Transform _transform;


    Rigidbody _body;
    Vector3 _originalPosition;

    bool ILickable.IsEatable => true;
    GameObject ILickable.AttachedObject { get { return this.gameObject; } }

    private Material[] _defaultMaterials;

    [SerializeField]
    private Material _previewMaterialPlacable;

    [SerializeField]
    private Material _previewMaterialNonPlacable;

    [SerializeField]
    private float _dropDistanceFromPlayer;

    [SerializeField]
    private LayerMask _collisionCheckLayer;

    [SerializeField]
    Collider _collision;

    [SerializeField]
    private Vector3 _respawnOffset;

    [SerializeField]
    Renderer[] _models;

    public event EventHandler<EventArgs> Eaten;

    public event EventHandler<EventArgs> Released;

    private bool _inCollision;

    private void Awake()
    {
        _transform = this.transform;

        _defaultMaterials = new Material[_models.Length];

        for (int i = 0; i < _models.Length; i++)
        {
            _defaultMaterials[i] = _models[i].material;
        }
        
        _body = this.GetComponent<Rigidbody>();
        _originalPosition = _transform.position;
    }

    public void HoldingLicked(Transform playerTransform)
    {
        _transform.gameObject.SetActive(true);

        if (_inCollision)
        {
            ChangeMaterial(_previewMaterialNonPlacable);
        }
        else
        {
            ChangeMaterial(_previewMaterialPlacable);

        }


        _transform.position = playerTransform.position + playerTransform.forward * _dropDistanceFromPlayer;
    }

    public  void Licked(Transform playerTransform)
    {
        _transform.gameObject.SetActive(false);
        _collision.enabled = false;
        _inCollision = false;
        _body.useGravity = false;
        OnEaten(EventArgs.Empty);
    }

    private void ChangeMaterial(Material material)
    {
        if(material == null)
        {
            for (int i = 0; i < _models.Length; i++)
            {
                _models[i].material = _defaultMaterials[i];
            }
        }
        else
        {
            foreach (var model in _models)
            {
                model.material = material;
            }
        }
    }

    private void ChangeMaterial() => ChangeMaterial(null);

    public  bool LickedReleased(Transform playerTransform)
    {


        if (_inCollision)
        {
            _inCollision = false;
            _transform.gameObject.SetActive(false);
            return false;
        }
        else
        {
            _transform?.gameObject.SetActive(true);
            ChangeMaterial();
            _collision.enabled = true;

            _transform.position = playerTransform.position + playerTransform.forward * _dropDistanceFromPlayer;
            _body.useGravity = true;

            _inCollision = false;
            OnReleased(EventArgs.Empty);

            return true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if ((_collisionCheckLayer & (1 << collisionObject.layer)) != 0)
        {
            _inCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collisionObject = other.gameObject;

        if ((_collisionCheckLayer & (1 << collisionObject.layer)) != 0)
        {
            _inCollision = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position,transform.position + _respawnOffset);
    }

    public void Respawn()
    {
        _transform.position = _originalPosition + _respawnOffset;
    }

    private void OnEaten(EventArgs eventArgs)
    {
        var handler = Eaten;
        handler?.Invoke(this, eventArgs);
    }

    private void OnReleased(EventArgs eventArgs)
    {
        var handler = Released;
        handler?.Invoke(this, eventArgs);
    }
}
