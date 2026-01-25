using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] float _timeToWait = 2f;

    MeshRenderer _meshRenderer;
    Rigidbody _rb;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();

        _meshRenderer.enabled = false;
        _rb.useGravity = false;
    }

    void Update()
    {
        if (Time.time > _timeToWait)
        {
            _meshRenderer.enabled = true;
            _rb.useGravity = true;
        }
    }
}
