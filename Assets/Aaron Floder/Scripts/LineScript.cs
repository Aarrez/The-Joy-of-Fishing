using System;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    private DistanceJoint2D distJoint2d;

    private Transform boat;

    [Header("Speed when reeling up and down")]
    [SerializeField] private float reelUp = 0.02f;

    [SerializeField] private float reelDown = 0.04f;

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

    private void FixedUpdate()
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
                if (distJoint2d.distance < 22)
                    distJoint2d.distance += Time.fixedDeltaTime * 2;
                break;

            case 1:
                distJoint2d.distance -= reelUp;
                break;

            case -1:
                if (distJoint2d.distance < 22)
                    distJoint2d.distance += reelDown;
                break;
        }
    }
}