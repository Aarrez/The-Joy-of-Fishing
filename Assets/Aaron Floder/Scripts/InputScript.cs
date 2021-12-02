using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    private TheJoyofFishing joyOfFishing;

    public static event Action DoMove;

    public static event Action MoveLine;

    public static Func<InputAction.CallbackContext> MoveCtx, LineCtx;

    private void Awake()
    {
        joyOfFishing = new TheJoyofFishing();
    }

    private void OnEnable()
    {
        joyOfFishing.Player.MoveBoat.performed += context =>
        {
            MoveCtx = delegate () { return context; };
            DoMove?.Invoke();
        };

        joyOfFishing.Player.MoveBoat.canceled += context =>
        {
            MoveCtx = delegate () { return context; };
            DoMove?.Invoke();
        };

        joyOfFishing.Player.MoveBoat.Enable();

        joyOfFishing.Player.FishingBait.performed += ctx =>
        {
            LineCtx = delegate () { return ctx; };
            MoveLine?.Invoke();
        };

        joyOfFishing.Player.FishingBait.canceled += ctx =>
        {
            LineCtx = delegate () { return ctx; };
            MoveLine?.Invoke();
        };

        joyOfFishing.Player.FishingBait.Enable();
    }

    private void OnDisable()
    {
        joyOfFishing.Player.MoveBoat.Disable();

        joyOfFishing.Player.FishingBait.Disable();
    }
}