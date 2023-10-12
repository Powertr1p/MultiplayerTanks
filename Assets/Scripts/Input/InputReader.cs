using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    public event Action<bool> PrimaryFiring;
    public event Action<Vector2> Moving;
    
    private Controls _controls;
    
    private void OnEnable()
    {
        if (ReferenceEquals(_controls, null))
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        
        _controls.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Moving?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            PrimaryFiring?.Invoke(true);
        else if (context.canceled)
            PrimaryFiring?.Invoke(false);
    }
}
