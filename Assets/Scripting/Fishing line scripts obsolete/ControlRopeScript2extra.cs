using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlRopeScript2extra : MonoBehaviour
{
    GameObject getRope;
    CrankScript2extra getCrank;

    TheJoyofFishing getKey;
    InputAction actionReelUp;
    InputAction actionReelDown;

    //InputAction actionSwingLeft;
    //InputAction actionSwingRight;


    //swing mechanics start
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    public float pushForce = 10f;

    public bool attached = true;
    public Transform attachedTo;
    private GameObject disregard;





    void Awake()
    {
        getKey = new TheJoyofFishing();
        //actionReelUp = getKey.Player.ReelUp;
        //actionReelDown = getKey.Player.ReelDown;
        //actionSwingLeft = getKey.Player.SwingLeft;
        //actionSwingRight = getKey.Player.SwingRight;


        getRope = GameObject.Find("CrankTest");
        getCrank = getRope.GetComponent<CrankScript2extra>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        hj = gameObject.GetComponent<HingeJoint2D>();

    }


    void Update()
    {
        CheckCrankUp();
        CheckCrankDown();
        //CheckSwingLeft();
        //CheckSwingRight();
    }

    void CheckCrankUp()
    {
        getCrank.Rotate(-1);
    }
    void CheckCrankDown()
    {
        getCrank.Rotate(1);
    }

    //void CheckSwingLeft()
    //{
    //    rb.AddRelativeForce(new Vector3(-1, 0, 0) * pushForce);
    //}

    //void CheckSwingRight()
    //{
    //    rb.AddRelativeForce(new Vector3(1, 0, 0) * pushForce);
    //}

    void OnEnable()
    {
        actionReelUp.Enable();
        actionReelDown.Enable();
        //actionSwingLeft.Enable();
        //actionSwingRight.Enable();

        actionReelUp.performed += _ => CheckCrankUp();
        actionReelDown.performed += _ => CheckCrankDown();
        //actionSwingLeft.performed += _ => CheckSwingLeft();
        //actionSwingRight.performed += _ => CheckSwingRight();
    }

    void OnDisable()
    {
        actionReelUp.Disable();
        actionReelDown.Disable();
        //actionSwingLeft.Disable();
        //actionSwingRight.Disable();
    }
}
