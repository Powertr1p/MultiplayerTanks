using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.Player
{
    public class InputHandler : NetworkBehaviour
    {
        public event Action PrimaryFireStart;
        public event Action PrimaryFireEnd;
        
        private Camera _camera;
        
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

        public void BindSceneCamera()
        {
            _camera = Camera.main;
        }

        public Vector2 GetAimInput()
        {
            var screenPosition = _camera.ScreenToWorldPoint(_inputReader.AimPosition);

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