using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour, IUpdate
{
    [Header("<color=blue>Components</color>")]
    [SerializeField] private Transform _target;

    private Vector3 _offset = new(), _finalCamPos = new();

    private void Start()
    {
        UpdateManager.Instance.AddObject(this);

        _offset = transform.position - _target.position;
    }

    public void ArtUpdate() { }

    public void ArtFixedUpdate() { }

    public void ArtLateUpdate()
    {
        _finalCamPos = _target.position + _offset;

        transform.position = _finalCamPos;
    }
}
