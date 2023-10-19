using Helpers.Interfaces;
using Unity.Netcode;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 5;

    private ulong _ownerClientId;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out NetworkObject networkObject))
            if (networkObject.OwnerClientId == _ownerClientId) return;

        if (other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(_damage);
        }
    }

    public void SetOwner(ulong ownerClientId)
    {
        _ownerClientId = ownerClientId;
    }
}
