using Core.Player;
using Unity.Netcode;
using UnityEngine;

public class Aiming : NetworkBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private Transform _turretTransform;

    private void LateUpdate()
    {
        if (!IsOwner) return;

        var turretPosition = _turretTransform.position;
        var mousePosition = _input.GetAimInput();
        
        _turretTransform.up = new Vector2(mousePosition.x - turretPosition.x, mousePosition.y - turretPosition.y);
    }
}
