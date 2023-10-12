using Unity.Netcode;
using UnityEngine;

namespace Core.Player
{
    public class InputHandler : NetworkBehaviour
    {
        private Camera _camera;
        
        private InputReader _inputReader;
        private Vector2 _lastMovementInput;

        private void Awake()
        {
            _inputReader ??= new InputReader();
            _camera = Camera.main;
        }
        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _inputReader.Moving += HandleMove;
        }

        public Vector2 GetMovementInput()
        {
            return _lastMovementInput;
        }

        public Vector2 GetAimInput()
        {
            var screenPosition = _camera.ScreenToWorldPoint(_inputReader.AimPosition);

            return screenPosition;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner) return;

            _inputReader.Moving -= HandleMove;
        }
        
        private void HandleMove(Vector2 input)
        {
            _lastMovementInput = input;
        }
    }
}