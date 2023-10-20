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
    [SerializeField] private Wallet _wallet;
    
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _muzzleFlashDuration;

    [Header("Settings")] 
    [SerializeField] private float _speed;
    [SerializeField] private int _costToFire;

    private bool _isFiring;
    private float _muzzleFlashTimer;
    private float _timer;

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
        TryDisableMuzzleFlash();
        
        if (!IsOwner) return;

        if (_timer > 0)
            _timer -= Time.deltaTime;
        
        if (!_isFiring) return;
        if (!CanFire()) return;
        if (_wallet.Coins.Value < _costToFire) return;

        var spawnPoint = _spawnPoint.position;
        var direction = _spawnPoint.up;
        
        PrimaryFireServerRpc(spawnPoint, direction);
        SpawnDummyProjectile(spawnPoint, direction);
        
        _timer = 1 / _fireRate;
    }

    private bool CanFire()
    {
        return _timer <= 0 ;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 direction)
    {
        if (_wallet.Coins.Value < _costToFire) return;
        
        _wallet.SpentCoins(_costToFire);
        
        var instance = SpawnProjectile(_serverProjectile, spawnPosition, direction);
        
        if (instance.TryGetComponent(out DamageDealer damageDealer))
            damageDealer.SetOwner(OwnerClientId);
        
        if (instance.TryGetComponent(out Rigidbody2D rb))
            SetProjectileVelocity(rb);
        
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
        EnableMuzzleFlash();
        var instance = SpawnProjectile(_clientProjectile, spawnPosition, direction);
        
        if (instance.TryGetComponent(out Rigidbody2D rb))
            SetProjectileVelocity(rb);
    }

    private void EnableMuzzleFlash()
    {
        _muzzleFlash.SetActive(true);
        _muzzleFlashTimer = _muzzleFlashDuration;
    }
    
    private void TryDisableMuzzleFlash()
    {
        if (_muzzleFlashTimer > 0f)
        {
            _muzzleFlashTimer -= Time.deltaTime;

            if (_muzzleFlashTimer <= 0f)
            {
                _muzzleFlash.SetActive(false);
            }
        }
    }

    private GameObject SpawnProjectile(GameObject projectile, Vector3 spawnPosition, Vector3 direction)
    {
        var instance = Instantiate(projectile, spawnPosition, Quaternion.identity);
        instance.transform.up = direction;
        
        Physics2D.IgnoreCollision(_playerCollider, instance.GetComponent<Collider2D>());

        return instance;
    }

    private void SetProjectileVelocity(Rigidbody2D rb)
    {
        rb.velocity = rb.transform.up * _speed;
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
