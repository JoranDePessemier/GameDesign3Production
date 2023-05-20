using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LickEatable : MonoBehaviour, ILickable, IRespawn
{
    Transform _transform;
    Collider _collision;
    Renderer _renderer;
    Rigidbody _body;
    Vector3 _originalPosition;

    bool ILickable.IsEatable => true;
    GameObject ILickable.AttachedObject { get { return this.gameObject; } }

    private Material _defaultMaterial;

    [SerializeField]
    private Material _previewMaterialPlacable;

    [SerializeField]
    private Material _previewMaterialNonPlacable;

    [SerializeField]
    private float _dropDistanceFromPlayer;

    [SerializeField]
    private LayerMask _collisionCheckLayer;

    [SerializeField]
    private Vector3 _respawnOffset;

    private bool _inCollision;

    private void Awake()
    {
        _transform = this.transform;
        _collision = this.GetComponent<Collider>();
        _renderer = this.GetComponentInChildren<Renderer>();
        _defaultMaterial = _renderer.material;
        _body = this.GetComponent<Rigidbody>();
        _originalPosition = _transform.position;
    }

    public void HoldingLicked(Transform playerTransform)
    {
        _transform.gameObject.SetActive(true);

        if (_inCollision)
        {
            _renderer.material = _previewMaterialNonPlacable;
        }
        else
        {
            _renderer.material = _previewMaterialPlacable;

        }


        _transform.position = playerTransform.position + playerTransform.forward * _dropDistanceFromPlayer;
    }

    public  void Licked(Transform playerTransform)
    {
        _transform.gameObject.SetActive(false);
        _collision.enabled = false;
        _inCollision = false;
        _body.useGravity = false;
    }

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
            _transform.gameObject.SetActive(true);
            _renderer.material = _defaultMaterial;
            _collision.enabled = true;

            _transform.position = playerTransform.position + playerTransform.forward * _dropDistanceFromPlayer;
            _body.useGravity = true;

            _inCollision = false;
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
}
