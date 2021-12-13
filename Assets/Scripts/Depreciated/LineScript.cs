using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LineScript : MonoBehaviour
{
    private AnimationCurve reelUpCurve;
    private AnimationCurve reelDownCurve;

    [Header("Reel Speed and RampUp time")]
    [SerializeField] private float reelSpeed = 1f;

    //[Min(0.02f), SerializeField] private float rampUpTime = 1f;

    //[Header("Line Distance")]
    //[SerializeField] private float maxLineDist = 22f;

    private float currentTime = 0f;
    private Vector2 inputVector = Vector2.zero;

    private void Awake()
    {
    }

    private void Start()
    {
        AddKeys();
    }

    #region Add keys to animation curve

    //Creates an animation curve with keyframes
    private void AddKeys()
    {
        //reelUpCurve = new AnimationCurve(new Keyframe(0f, 0.02f), new Keyframe(rampUpTime, 1f));
        //reelUpCurve.preWrapMode = WrapMode.PingPong;
        //reelUpCurve.postWrapMode = WrapMode.PingPong;

        //reelDownCurve = new AnimationCurve(new Keyframe(rampUpTime, 1f), new Keyframe(0f, 0.02f));
        //reelUpCurve.preWrapMode = WrapMode.PingPong;
        //reelUpCurve.postWrapMode = WrapMode.PingPong;
    }

    #endregion Add keys to animation curve

    //GetInput method is called when WS or UpDown arrow keys are pressed
    private void OnEnable()
    {
        InputScript.MoveLine += GetInput;
    }

    //Called when object is destroyed of disabled
    private void OnDisable()
    {
        InputScript.MoveLine -= GetInput;
    }

    private void Update()
    {
        HookMovement();
    }

    private void GetInput()
    {
        inputVector = InputScript.LineCtx().ReadValue<Vector2>();
    }

    private void HookMovement()
    {
        transform.Translate(inputVector * reelSpeed * Time.fixedDeltaTime);
    }
}