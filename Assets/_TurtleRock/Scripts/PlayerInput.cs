using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputActionsAsset;
[CreateAssetMenu(fileName = "New Input Reader,", menuName = "Input/InputReader")]
public class PlayerInput : ScriptableObject, IPlayerActions
{
    //Private
    private InputActionsAsset _input;
    //public
    public event Action<Vector2> MovementEvent;
    public event Action JumpEvent;
    public void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        if (_input == null)
        {
            _input = new InputActionsAsset();
            _input.Player.SetCallbacks(this);
        }
        _input.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent.Invoke();
        }
    }
}
