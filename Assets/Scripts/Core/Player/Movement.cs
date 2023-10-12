using Core.Player;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private Transform _body;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _turningRate = 30f;

    private void Update()
    {
        if (!IsOwner) return;

        float zRotation = _input.GetMovementInput().x * -_turningRate * Time.deltaTime;
        _body.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        _rb.velocity = _body.up * (_input.GetMovementInput().y * _speed);
    }
}
