using System;
using Helpers.Interfaces;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour, IDamagable
{
    public event Action<Health> OnDie;

    public int MaxHealth => _maxHealth;
    public NetworkVariable<int> CurrentHealth => _currentHealth;

    [SerializeField] private int _maxHealth = 100;
    private NetworkVariable<int> _currentHealth = new NetworkVariable<int>();

    private bool _isDead;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        ModifyHealth(-amount);
    }

    public void RestoreHealth(int amount)
    {
        ModifyHealth(amount);
    }

    private void ModifyHealth(int value)
    {
        if (_isDead) return;

        _currentHealth.Value = Mathf.Clamp(_currentHealth.Value + value, 0, MaxHealth);

        if (_currentHealth.Value == 0)
        {
            _isDead = true;
            OnDie?.Invoke(this);
        }
    }
}
