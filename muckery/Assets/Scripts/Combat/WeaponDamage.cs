using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider _playerCollider;

    int _damageAmount;
    float _knockback;

    List<Collider> _alreadyCollidedWith = new List<Collider>();//TODO: find better naming

    void OnEnable()
    {
        _alreadyCollidedWith.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == _playerCollider) { return; }

        if (_alreadyCollidedWith.Contains(other)) { return; }

        _alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(_damageAmount);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - _playerCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * _knockback);
        }
    }

    public void SetAttack(int damageAmount, float knockback)
    {
        this._damageAmount = damageAmount;
        this._knockback = knockback;
    }
}