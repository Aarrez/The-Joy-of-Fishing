using UnityEngine;
using UnityEngine.InputSystem;
public class BoatScript : MonoBehaviour
{
    //Privates
    [Min(0.02f)] [SerializeField] private float rampUpTime = 1f;

    float maxInterval = 0.4f;
    float minInterval = 0.05f;
    float lastInstanceTime = 0;
    private float triggerValue;
    private Vector2 moveInput, hookInput;
    private bool BoatCanMove = true;
    private GameObject curHook;
    private Rigidbody2D rig2d;
    private AnimationCurve moveAnimCurve;
    public int maxLineLength;
    public bool currentlyReelingUp;

    //Publics
    [Header("Movement stuff")]
    [SerializeField] public float boatSpeed = 1f, BoatSpeedForce = 10f, forcetoAdd = 100;

    public static event System.Action<bool> IsFishing;
    public static event System.Action DoneFishing;

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
        //MakeAnimationCurve();
        //GetKey = new TheJoyofFishing();
        //GetKey.Enable();
    }

    #region Make a animation curve

    private void MakeAnimationCurve()
    {
        moveAnimCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(rampUpTime, 1f));
        moveAnimCurve.preWrapMode = WrapMode.PingPong;
        moveAnimCurve.postWrapMode = WrapMode.PingPong;
    }

    #endregion Make a animation curve

    private void FixedUpdate()
    {
        MoveLeftRight();
        moveHook();
        ReelRope();
    }




    public void GetInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void GetTriggerValues(InputAction.CallbackContext context)
    {
        triggerValue = context.ReadValue<float>();
    }

    public void GetRightStickInput(InputAction.CallbackContext context)
    {
        hookInput = context.ReadValue<Vector2>();
    }
    public void CastOutButton(InputAction.CallbackContext context)
    {
        if(context.performed && ropeActive == false)
        {
            OnCastOut();
        }
    }

    public void CallShop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.onShopSwitch();
        }
    }
    private void MoveLeftRight()
    {
        if (!BoatCanMove) { return; }

        else
        {
            bool playerhashorizontalspeed = Mathf.Abs(rig2d.velocity.x) > Mathf.Epsilon;
            Vector2 playerVelocity = new Vector2(moveInput.x * BoatSpeedForce, rig2d.velocity.y);
            rig2d.velocity = playerVelocity;
        }
    }


    public void moveHook()
    {
        if (RopeScript.instance.hookRigidbody2D == null) 
        {  
            return;
        }

        else if (RopeScript.instance.hookRigidbody2D != null)
        {
            RopeScript.instance.hookRigidbody2D.AddForce(new Vector2(hookInput.x, hookInput.y) * forcetoAdd * Time.deltaTime);
        }
    }
    public void ReelRope()
    {
        if (RopeScript.instance.hookRigidbody2D == null) 
        {  
            return;   
        }

        else
        {
            float rightTrigger = Mathf.Clamp(triggerValue, 0, 1);
            float leftTrigger = Mathf.Abs(Mathf.Clamp(triggerValue, -1, 0));

            if (rightTrigger > 0)
            {
                // reel out
                if (Time.time > lastInstanceTime + Mathf.Lerp(maxInterval, minInterval, rightTrigger))
                {
                    lastInstanceTime = Time.time;
                    RopeScript.instance.CreateNode();
                }
            }
            else if (leftTrigger > 0)
            {
                if (Time.time > lastInstanceTime + Mathf.Lerp(maxInterval, minInterval, leftTrigger))
                {
                    lastInstanceTime = Time.time;
                    RopeScript.instance.DestroyNode();
                }
            }   
        }
    
    }



//========================================Rope Stuff======================================================
    public void OnCastOut()
    {
        //when rope is not activated
        if (ropeActive == false && GameManager.instance.moveCam == 1)
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

        //Sends out a message for other scripts to listen
        DoneFishing?.Invoke();
    }
}

























//=============================OLD CODE=====================================


    /*private void OnEnable()
    {
        InputScript.DoMove += GetInput;
        IsFishing += delegate (bool theFishing) { BoatCanMove = theFishing; };
    }

    private void OnDisable()
    {
        InputScript.DoMove -= GetInput;
        IsFishing -= delegate (bool theFishsing) { BoatCanMove = theFishsing; };
    }*/


    /*private void Update()
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
        if (reelfloatup == Vector2.zero && reelfloatdown == Vector2.zero)
        {
            currentlyReelingUp = false;
        }
        if (reelfloatup == Vector2.up && ropeActive == true && elapsed >= 0.12f && rig2d.velocity.x.Equals(0) && boostbool == false)
        {
            elapsed = elapsed % 0.12f; //polish would need floats in these statements varaibled so changeability with upgrades can be done.
            RopeScript.instance.DestroyNode();
            currentlyReelingUp = true;
        }
        else if (reelfloatup == Vector2.up && boostbool == true && elapsed >= 0.06f && ropeActive == true && rig2d.velocity.x.Equals(0))
        {
            elapsed = elapsed % 0.06f;
            RopeScript.instance.DestroyNode();
            currentlyReelingUp = true;
        }

        if (reelfloatdown == Vector2.down && ropeActive == true && elapsed >= 0.12f && rig2d.velocity.x.Equals(0) && RopeScript.instance.Nodes.Count < maxLineLength)
        {
            elapsed = elapsed % 0.12f;
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
    }*/

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

        /*private void MoveLeftRight()
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
    }*/