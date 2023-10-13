using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : Controls.IPlayerActions
{
    public event Action PrimaryFireButtonPressed;
    public event Action PrimaryFireButtonReleased;
    public event Action<Vector2> Moving;
    
    public Vector2 AimPosition { get; private set; }
    
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
            PrimaryFireButtonPressed?.Invoke();
        else if (context.canceled)
            PrimaryFireButtonReleased?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}
