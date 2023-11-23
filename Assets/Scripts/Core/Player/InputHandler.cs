using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.Player
{
    public class InputHandler : NetworkBehaviour
    {
        public event Action PrimaryFireStart;
        public event Action PrimaryFireEnd;

        private InputReader _inputReader;
        private Vector2 _lastMovementInput;

        private void Awake()
        {
            _inputReader ??= new InputReader();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _inputReader.Moving += HandleMove;
            _inputReader.PrimaryFireButtonPressed += OnFireButtonPressed;
            _inputReader.PrimaryFireButtonReleased += OnFireButtonReleased;
        }
        
        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;

            _inputReader.Moving -= HandleMove;
            _inputReader.PrimaryFireButtonPressed -= OnFireButtonPressed;
            _inputReader.PrimaryFireButtonReleased -= OnFireButtonReleased;
        }

        public Vector2 GetMovementInput()
        {
            return _lastMovementInput;
        }

        public Vector2 GetAimInput(Camera sceneCamera)
        {
            var screenPosition = sceneCamera.ScreenToWorldPoint(_inputReader.AimPosition);

            return screenPosition;
        }
        
        private void HandleMove(Vector2 input)
        {
            _lastMovementInput = input;
        }

        private void OnFireButtonPressed()
        {
            PrimaryFireStart?.Invoke();
        }

        private void OnFireButtonReleased()
        {
            PrimaryFireEnd?.Invoke();
        }
    }
}