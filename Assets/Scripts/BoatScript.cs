using UnityEngine;
using UnityEngine.InputSystem;
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
    //hook prefab
    public GameObject hook;
    public Transform baitpoint;
    //holds whether rope is active or not
    bool ropeActive;
    //current hook on the scene
    GameObject curHook;




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

	void OnReelUp(InputValue value)
	{
        if(value.isPressed == true)
        {
            RopeScript.instance.DestroyNode();
            //RopeScript.instance.crankdown();
        }
        return;
	}
    void OnReelDown(InputValue value)
    {
        if(value.isPressed == true)
        {
            RopeScript.instance.CreateNode();
            //RopeScript.instance.crankdown();
        }
        return;
    }


    private void OnCastOut()
    {
        //when rope is not activated
        if (ropeActive == false)
        {
            //destiny is where the mouse is
            Vector2 destiny = baitpoint.position;   //Camera.main.ScreenToWorldPoint (Input.mousePosition);

            //creates a hook
            curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

            //sets its destiny
            curHook.GetComponent<RopeScript>().destiny = destiny;
            GameManager.instance.Hook = curHook.transform;
            GameManager.instance.baitCam = true;
            GameManager.instance.moveCam = 3;
            //sets rope to enabled
            ropeActive = true;
        }
        else
        {

            //delete rope
            Destroy(curHook);
            GameManager.instance.baitCam = false;
            GameManager.instance.moveCam = 1;
            //sets rope to disabled
            ropeActive = false;

        }
    }
}