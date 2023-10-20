using Helpers.Interfaces;
using Unity.Netcode;
using UnityEngine;

public class Wallet : NetworkBehaviour
{
    //public for debugging
    public NetworkVariable<int> Coins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            var collectedCoin =  collectable.Collect();
            
            if (!IsServer) return;

            Coins.Value += collectedCoin;
        }
    }
}
