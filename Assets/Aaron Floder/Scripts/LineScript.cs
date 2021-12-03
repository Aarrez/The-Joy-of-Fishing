using System;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    private DistanceJoint2D distJoint2d;

    private Transform boat;

    [Header("Change gravity affects the bait")]
    [SerializeField] private float reelUp = 1f;

    [SerializeField] private float reelDown = 1f;

    private float inputValueY = 0f;

    private Vector2 InputVector = Vector2.zero;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
        distJoint2d = GetComponentInParent<DistanceJoint2D>();
        boat = GetComponentInParent<BoatScript>().transform;
    }

    private void OnEnable()
    {
        InputScript.DoMove += GetInput;
    }

    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
    }

    private void Update()
    {
        HookMovement();
    }

    private void GetInput()
    {
        InputVector = InputScript.MoveCtx().ReadValue<Vector2>();
        inputValueY = InputVector.y;
    }

    private void HookMovement()
    {
        switch (inputValueY)
        {
            case 0:
                distJoint2d.distance += Time.fixedDeltaTime;
                break;

            case 1:
                distJoint2d.distance -= reelUp;
                break;

            case -1:
                rig2d.gravityScale += reelDown;
                break;
        }
    }
}