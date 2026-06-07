using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;//TODO: should this be a float???

    int _currentHealth;
    bool _isInvulnerable = false;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => _currentHealth == 0;

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this._isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damageAmount)
    {
        if (_currentHealth <= 0) { return; }

        if (_isInvulnerable) { return; }

        _currentHealth = Mathf.Max( _currentHealth - damageAmount, 0); //returns whatever number is higher

        OnTakeDamage?.Invoke();

        if (_currentHealth == 0)
        {
            OnDie?.Invoke();
        }
    }
}