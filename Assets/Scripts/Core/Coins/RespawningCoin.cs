using System;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;

    private Vector3 _lastPosition;

    private void Update()
    {
        if (_lastPosition != transform.position)
            Show(true);

        _lastPosition = transform.position;
    }

    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }
        
        if (AlreadyCollected) return 0;
        
        AlreadyCollected = true;
        
        OnCollected?.Invoke(this);
        
        return CoinValue;
    }

    public void Reset()
    {
        AlreadyCollected = false;
    }
}
