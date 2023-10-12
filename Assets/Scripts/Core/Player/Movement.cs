using System;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _body;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _turningRate = 30f;

    private Vector2 _lastInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _inputReader.Moving += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _inputReader.Moving -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner) return;

        float zRotation = _lastInput.x * -_turningRate * Time.deltaTime;
        _body.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _rb.velocity = _body.up * (_lastInput.y * _speed);
    }

    private void HandleMove(Vector2 input)
    {
        _lastInput = input;
    }
}
