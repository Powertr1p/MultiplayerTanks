using System;
using Core.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aiming : NetworkBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private NetworkInitializer _initializer;

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
        if (!_initializer.IsInitialized) return;

        var turretPosition = _turretTransform.position;
        var mousePosition = _input.GetAimInput();

        _turretTransform.up = new Vector2(mousePosition.x - turretPosition.x, mousePosition.y - turretPosition.y);
    }

    private void InitializePlayerToSceneReferences()
    {
        _input.BindSceneCamera();
    }
}
