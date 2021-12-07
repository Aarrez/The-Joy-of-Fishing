using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LineScript : MonoBehaviour
{
    private DistanceJoint2D distJoint2d;
    private AnimationCurve reelUpCurve;
    private AnimationCurve reelDownCurve;

    [Header("Reel Speed and RampUp time")]
    [SerializeField] private float reelSpeed = 0.1f;

    [Min(0.02f), SerializeField] private float rampUpTime = 1f;

    [Header("Line Distance")]
    [SerializeField] private float maxLineDist = 22f;

    private float currentTime = 0f;
    private int inputValueY = 0;

    private void Awake()
    {
        distJoint2d = FindObjectOfType<DistanceJoint2D>();
    }

    private void Start()
    {
        AddKeys();
    }

    #region Add keys to animation curve

    //Creates an animation curve with keyframes
    private void AddKeys()
    {
        reelUpCurve = new AnimationCurve(new Keyframe(0f, 0.02f), new Keyframe(rampUpTime, 1f));
        reelUpCurve.preWrapMode = WrapMode.PingPong;
        reelUpCurve.postWrapMode = WrapMode.PingPong;

        reelDownCurve = new AnimationCurve(new Keyframe(rampUpTime, 1f), new Keyframe(0f, 0.02f));
        reelUpCurve.preWrapMode = WrapMode.PingPong;
        reelUpCurve.postWrapMode = WrapMode.PingPong;
    }

    #endregion Add keys to animation curve

    //GetInput method is called when WS or UpDown arrow keys are pressed
    private void OnEnable()
    {
        InputScript.MoveLine += GetInput;
    }

    private void OnDisable()
    {
        InputScript.MoveLine -= GetInput;
    }

    private void FixedUpdate()
    {
        HookMovement();
    }

    private void GetInput()
    {
        inputValueY = (int)InputScript.LineCtx().ReadValue<float>();
    }

    private void HookMovement()
    {
        switch (inputValueY)
        {
            //Constant increse in distance from boat if no input
            case 0:
                if (distJoint2d.distance <= maxLineDist)
                {
                    currentTime = Mathf.Clamp(currentTime, 0.02f, rampUpTime);
                    distJoint2d.distance += Time.fixedDeltaTime;
                    currentTime -= Time.fixedDeltaTime;
                }

                break;
            //Decreses the distance from the boat if pressing W or Up Arrow key is pressed
            case 1:
                currentTime = Mathf.Clamp(currentTime, 0.02f, rampUpTime);
                currentTime += Time.fixedDeltaTime;
                distJoint2d.distance -= reelUpCurve.Evaluate(currentTime) * reelSpeed;

                break;
            //Faster increse of distance from the boat if the S or Down Arrow key is pressed
            case -1:
                if (distJoint2d.distance <= maxLineDist)
                {
                    currentTime = Mathf.Clamp(currentTime, 0f, rampUpTime);
                    currentTime += Time.fixedDeltaTime;
                    distJoint2d.distance += reelDownCurve.Evaluate(currentTime) * reelSpeed;
                }

                break;
        }
    }
}