using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : Controls.IPlayerActions
{
    public event Action<bool> PrimaryFiring;
    public event Action<Vector2> Moving;
    
    public InputReader()
    {
        var controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
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
