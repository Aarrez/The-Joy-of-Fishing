using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    private TheJoyofFishing joyOfFishing;

    public static event Action DoMove;

    public static Func<InputAction.CallbackContext> MoveCtx;

    private void Awake()
    {
        joyOfFishing = new TheJoyofFishing();
    }

    private void OnEnable()
    {
        joyOfFishing.Player.Move.performed += context =>
        {
            MoveCtx = delegate () { return context; };
            DoMove?.Invoke();
        };

        joyOfFishing.Player.Move.canceled += context =>
        {
            MoveCtx = delegate () { return context; };
            DoMove?.Invoke();
        };

        joyOfFishing.Player.Move.Enable();
    }

    private void OnDisable()
    {
        joyOfFishing.Player.Move.Disable();
    }
}