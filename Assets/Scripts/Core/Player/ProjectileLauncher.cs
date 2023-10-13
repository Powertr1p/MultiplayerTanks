using Core.Player;
using Unity.Netcode;
using UnityEngine;


public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _serverProjectile;
    [SerializeField] private GameObject _clientProjectile;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private InputHandler _input;

    [Header("Settings")] 
    [SerializeField] private float _speed;

    private bool _isFiring;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _input.PrimaryFireStart += FireProjectile;
        _input.PrimaryFireEnd += EndFire;
    }
    
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        
        _input.PrimaryFireStart -= FireProjectile;
        _input.PrimaryFireEnd -= EndFire;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!_isFiring) return;
        
        PrimaryFireServerRpc(_spawnPoint.position, _spawnPoint.up);
        SpawnDummyProjectile(_spawnPoint.position, _spawnPoint.up);
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 direction)
    {
        var instance = Instantiate(_serverProjectile, spawnPosition, Quaternion.identity);
        instance.transform.up = direction;
        
        SpawnDummyProjectileClientRpc(spawnPosition, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 direction)
    {
        if (IsOwner) return;
        
        SpawnDummyProjectile(spawnPosition, direction);
    }

    private void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 direction)
    {
        var instance = Instantiate(_clientProjectile, spawnPosition, Quaternion.identity);
        instance.transform.up = direction;
    }
    
    private void FireProjectile()
    {
        _isFiring = true;
    }
    
    private void EndFire()
    {
        _isFiring = false;
    }
}
