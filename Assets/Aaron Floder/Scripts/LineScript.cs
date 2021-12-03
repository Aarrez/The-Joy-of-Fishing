using System;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    private DistanceJoint2D distJoint2d;
    private float reelTime;

    [Header("Speed when reeling up and down")]
    [SerializeField] private float reelUp = 0.02f;

    [SerializeField] private float reelDown = 0.04f;

    [Header("Animation reel curves")]
    [SerializeField] private AnimationCurve reelUpCurve;

    [SerializeField] private AnimationCurve reelDownCurve;

    private float inputValueY = 0f;

    private Vector2 InputVector = Vector2.zero;

    private void Awake()
    {
        distJoint2d = GetComponentInParent<DistanceJoint2D>();
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
                reelTime = 0;
                break;

            case 1:

                distJoint2d.distance -= reelUpCurve.Evaluate(reelTime);
                reelTime += reelUp;
                reelTime = Mathf.Clamp(reelTime, 0, 2);
                break;

            case -1:
                if (distJoint2d.distance < 22)
                {
                    distJoint2d.distance += reelDownCurve.Evaluate(reelTime);
                    reelTime -= reelDown;
                    reelTime = Mathf.Clamp(reelTime, 0, 2);
                }
                break;
        }
    }
}