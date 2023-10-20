using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin _coinPrefab;
    
    [SerializeField] private int _maxCoins = 10;
    [SerializeField] private int _coinValue = 10;
    
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private LayerMask _layer;

    private Collider2D[] _coinBuffer = new Collider2D[1];
    private float _coinRadius;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius;
        
        for (int i = 0; i < _maxCoins; i++)
            Spawn();
    }

    private void Spawn()
    {
        var instance =  Instantiate(_coinPrefab, GetSpawnPoint(), Quaternion.identity);
        instance.SetValue(_coinValue);
        instance.GetComponent<NetworkObject>().Spawn();

        instance.OnCollected += HandleCollected;
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;

        while (true)
        {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);

            int overlaps = Physics2D.OverlapCircleNonAlloc(spawnPoint, _coinRadius, _coinBuffer, _layer);

            if (overlaps == 0)
                return spawnPoint;
        }
    }
    
    private void HandleCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }
}
