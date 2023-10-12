using Unity.Netcode;
using UnityEngine;

namespace Core.Player
{
    public class InputHandler : NetworkBehaviour
    {
        public Vector2 MovementInput { get; private set; }
        
        private InputReader _inputReader;

        private void Awake()
        {
            _inputReader ??= new InputReader();
        }
        
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
        
        private void HandleMove(Vector2 input)
        {
            MovementInput = input;
        }
    }
}