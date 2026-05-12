using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] CharacterController _characterController;

    Collider[] _colliders;
    Rigidbody[] _rigidbodies;

    void Start()
    {
        _colliders = GetComponentsInChildren<Collider>(true); //TODO: this method is expensive. perhaps there is a better way?
        _rigidbodies = GetComponentsInChildren<Rigidbody>(true); //TODO: this method is expensive. perhaps there is a better way?

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in _colliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll")) //TODO: remove hardcoding + find more performant method
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll")) //TODO: remove hardcoding + find more performant method
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        _characterController.enabled = !isRagdoll;
        _animator.enabled = !isRagdoll;
    }
}