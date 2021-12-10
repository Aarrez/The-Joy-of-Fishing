using UnityEngine;

public class BoatScript : MonoBehaviour
{
    private Rigidbody2D rig2d;
    private AnimationCurve moveAnimCurve;

    [Header("Movement stuff")]
    [SerializeField] private float boatSpeed = 1f;

    [Min(0.02f)] [SerializeField] private float rampUpTime = 1f;

    private float inputValueX = 0f;

    private float currentTime;

    private bool BoatCanMove = true;

    public static event System.Action<bool> IsFishing;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MakeAnimationCurve();
    }

    #region Make a animation curve

    private void MakeAnimationCurve()
    {
        moveAnimCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(rampUpTime, 1f));
        moveAnimCurve.preWrapMode = WrapMode.PingPong;
        moveAnimCurve.postWrapMode = WrapMode.PingPong;
    }

    #endregion Make a animation curve

    private void OnEnable()
    {
        InputScript.DoMove += GetInput;
        IsFishing += delegate (bool theFishing) { BoatCanMove = theFishing; };

    }

    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
        IsFishing -= delegate (bool theFishsing) { BoatCanMove = theFishsing; };
    }

    private void FixedUpdate()
    {
        MoveLeftRight();
    }

    private void GetInput()
    {
        inputValueX = InputScript.MoveCtx().ReadValue<float>();
    }

    private void MoveLeftRight()
    {
        if (!BoatCanMove) { return; }

        switch (inputValueX)
        {
            case 0:
                currentTime = Mathf.Clamp01(currentTime);
                currentTime -= Time.fixedDeltaTime;
                float animEval = moveAnimCurve.Evaluate(currentTime);
                transform.Translate(new Vector2(inputValueX * animEval * Time.fixedDeltaTime, 0f));
                break;

            default:
                currentTime = Mathf.Clamp01(currentTime);
                currentTime += Time.fixedDeltaTime;
                animEval = moveAnimCurve.Evaluate(currentTime);
                transform.Translate(new Vector2(inputValueX * animEval * Time.fixedDeltaTime, 0f));
                break;
        }
    }
}