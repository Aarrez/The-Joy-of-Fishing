using UnityEngine;
public class BoatScript : MonoBehaviour
{
    //Privates
    [Min(0.02f)] [SerializeField] private float rampUpTime = 1f;

    private float currentTime, elapsed = 0f, inputValueX = 0f;
    private bool BoatCanMove = true;
    private GameObject curHook;
    private Rigidbody2D rig2d;
    private AnimationCurve moveAnimCurve;
    private TheJoyofFishing GetKey;

    public int maxLineLength;

    //Publics
    [Header("Movement stuff")]
    [SerializeField] public float boatSpeed = 1f, BoatSpeedForce = 10f;

    public static event System.Action<bool> IsFishing;

    //hook prefab
    public GameObject hook;

    public Transform baitpoint, baitTransform, rodpoint;

    //holds whether rope is active or not
    [HideInInspector] public bool ropeActive, boostbool;

    //current hook on the scene

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MakeAnimationCurve();
        GetKey = new TheJoyofFishing();
        GetKey.Enable();
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

    private void Update()
    {
        Vector2 reelfloatup = GetKey.Player.ReelUp.ReadValue<Vector2>();
        Vector2 reelfloatdown = GetKey.Player.ReelDown.ReadValue<Vector2>();
        Vector2 reelupboost = GetKey.Player.ReelUpBoost.ReadValue<Vector2>();
        elapsed += Time.deltaTime;

        if (reelupboost == Vector2.up)
        {
            boostbool = true;
        }
        else { boostbool = false; }

        if (reelfloatup == Vector2.up && ropeActive == true && elapsed >= 0.2f && rig2d.velocity.x.Equals(0) && boostbool == false)
        {
            elapsed = elapsed % 0.2f; //polish would need floats in these statements varaibled so changeability with upgrades can be done.
            RopeScript.instance.DestroyNode();
        }
        else if (boostbool == true && elapsed >= 0.1f && ropeActive == true && rig2d.velocity.x.Equals(0))
        {
            elapsed = elapsed % 0.1f;
            RopeScript.instance.DestroyNode();
        }

        if (reelfloatdown == Vector2.down && ropeActive == true && elapsed >= 0.2f && rig2d.velocity.x.Equals(0) && RopeScript.instance.Nodes.Count <= maxLineLength)
        {
            elapsed = elapsed % 0.2f;
            RopeScript.instance.CreateNode();
        }

        Vector2 boatleft = GetKey.Player.BoatLeft.ReadValue<Vector2>();
        Vector2 boatright = GetKey.Player.BoatRight.ReadValue<Vector2>();
        if (boatleft == Vector2.left)
        {
            rig2d.AddForce(new Vector2(boatleft.x, 0) * BoatSpeedForce * Time.deltaTime);
        }
        if (boatright == Vector2.right)
        {
            rig2d.AddForce(new Vector2(boatright.x, 0) * BoatSpeedForce * Time.deltaTime);
        }
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

    //void OnReelUp(InputValue value)
    //{
    //       if(value.isPressed == true && ropeActive == true)
    //       {
    //           RopeScript.instance.DestroyNode();
    //           //RopeScript.instance.crankdown();
    //       }
    //       return;
    //}
    //   void OnReelDown(InputValue value)
    //   {
    //       if(value.isPressed == true && ropeActive == true)
    //       {
    //           RopeScript.instance.CreateNode();
    //           //RopeScript.instance.crankdown();
    //       }
    //       return;
    //   }

    public void OnCastOut()
    {
        //when rope is not activated
        if (ropeActive == false)
        {
            //destiny is where the mouse is
            Vector2 destiny = baitpoint.position;
            //creates a hook
            curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
            curHook.transform.parent = rodpoint;
            //sets its destiny
            curHook.GetComponent<RopeScript>().destiny = destiny;

            GameManager.instance.Hook = curHook.transform;
            GameManager.instance.baitCam = true;
            GameManager.instance.moveCam = 3;
            //sets rope to enabled
            ropeActive = true;
        }
        //else //commennt this else statement to remove "click 0 again to delet rope"
        //{
        //    //delete rope
        //    DeleteRope();

        //}
    }

    public void DeleteRope()
    {
        Destroy(curHook);
        GameManager.instance.baitCam = false;
        GameManager.instance.moveCam = 1;
        //sets rope to disabled
        ropeActive = false;
    }
}