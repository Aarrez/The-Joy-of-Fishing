using System;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    [Header("Change gravity affects the bait")]
    [SerializeField] private float reelUpGravity = -1f;

    [SerializeField] private float reelDownGravity = 2f;

    [SerializeField] private float noInputGravity = 1f;

    private float inputValueY = 0f;

    private Vector2 InputVector = Vector2.zero;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputScript.DoMove += GetInput;
        InputScript.DoMove += GHookMovement;
    }

    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
        InputScript.DoMove -= GHookMovement;
    }

    private void GetInput()
    {
        InputVector = InputScript.MoveCtx().ReadValue<Vector2>();
        inputValueY = InputVector.y;
    }

    private void GHookMovement()
    {
        switch (inputValueY)
        {
            case 0:
                rig2d.gravityScale = noInputGravity;
                break;

            case 1:
                rig2d.gravityScale = reelUpGravity;
                break;

            case -1:
                rig2d.gravityScale = reelDownGravity;
                break;
        }
    }
}