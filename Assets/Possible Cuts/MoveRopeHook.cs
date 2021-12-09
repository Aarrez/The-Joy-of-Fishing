using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveRopeHook : MonoBehaviour
{
    TheJoyofFishing getKey;
    InputAction actionSwingLeft;
    InputAction actionSwingRight;

    public float pushForce = 10f;

    public Rigidbody2D rb;

    Keyboard kb;

    Transform gettransform;
    void Start()
    {
        getKey = new TheJoyofFishing();
        actionSwingLeft = getKey.Player.SwingLeft;
        actionSwingRight = getKey.Player.SwingRight;
        //actionSwingLeft.performed += ctx => CheckSwingLeft();
        //actionSwingRight.performed += ctx => CheckSwingRight();

        kb = InputSystem.GetDevice<Keyboard>();
        gettransform = GetComponent<Transform>();
    }


    void Update()
    {
       if (kb.leftArrowKey.wasPressedThisFrame)
        {
            CheckSwingLeft();
        }

        if (kb.rightArrowKey.wasPressedThisFrame)
        {
            CheckSwingRight();
        }
    }

    void CheckSwingLeft()
    {
        gettransform.transform.position = new Vector3(gettransform.position.x - 1, gettransform.position.y, gettransform.position.z);
        Debug.Log("LEFTPOLEAS");
    }

    void CheckSwingRight()
    {
        gettransform.transform.position = new Vector3(gettransform.position.x + 1, gettransform.position.y, gettransform.position.z);
        Debug.Log("RIGht P{LEASE");
    }


    void OnEnable()
    {
        actionSwingLeft.Enable();
        actionSwingRight.Enable();



    }

    void OnDisable()
    {

        actionSwingLeft.Disable();
        actionSwingRight.Disable();
    }
}
