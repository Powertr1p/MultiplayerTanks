using System;
using Core.Player;
using Unity.Netcode;
using UnityEngine;

public class Aiming : NetworkBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private NetworkInitializer _initializer;

    private Camera _camera;
    private bool _isInitialized;

    private void Start()
    {
        if (!IsHost)
        {
            _camera = Camera.main;
            _isInitialized = true;
        }
    }

    private void OnEnable()
    {
        _initializer.OnPlayerInitialized += InitializePlayerToSceneReferences;
    }

    private void OnDisable()
    {
        _initializer.OnPlayerInitialized -= InitializePlayerToSceneReferences;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;
        if (!_isInitialized) return;

        var turretPosition = _turretTransform.position;
        var mousePosition = _input.GetAimInput(_camera);

        _turretTransform.up = new Vector2(mousePosition.x - turretPosition.x, mousePosition.y - turretPosition.y);
    }

    private void InitializePlayerToSceneReferences()
    {
        Debug.Log("INIT");
        _isInitialized = true;
        
        _camera = Camera.main;
    }
}
