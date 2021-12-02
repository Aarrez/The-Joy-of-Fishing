using UnityEngine;

public class BoatScript : MonoBehaviour
{
    private Rigidbody2D rig2d;

    [SerializeField] private AnimationCurve moveForceCurve;

    private float boatSpeed = 0f;

    private float inputValueX = 0f;

    private float theTime;

    private float currentTime;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        var aKey = moveForceCurve.keys[moveForceCurve.length - 1];
        theTime = aKey.time;
        currentTime = theTime;
    }

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
        MoveLeftRight();
    }

    private void GetInput()
    {
        inputValueX = InputScript.LineCtx().ReadValue<float>();
        currentTime = theTime;
    }

    private void MoveLeftRight()
    {
        currentTime -= Time.fixedDeltaTime;
        boatSpeed = moveForceCurve.Evaluate(currentTime);
        rig2d.AddForce(new Vector2(inputValueX * boatSpeed, 0f), ForceMode2D.Force);
    }
}