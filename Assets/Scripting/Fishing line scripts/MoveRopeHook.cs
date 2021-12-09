using UnityEngine;
using UnityEngine.InputSystem;

public class MoveRopeHook : MonoBehaviour
{
    MoveRopeHook()
    {
    }

    private TheJoyofFishing getKey;
    private InputAction actionSwingLeft;
    private InputAction actionSwingRight;

    public float pushForce = 10f;

    public Rigidbody2D rb;

    private Keyboard kb;

    private Transform gettransform;

    private void Start()
    {
        getKey = new TheJoyofFishing();
        actionSwingLeft = getKey.Player.SwingLeft;
        actionSwingRight = getKey.Player.SwingRight;
        //actionSwingLeft.performed += ctx => CheckSwingLeft();
        //actionSwingRight.performed += ctx => CheckSwingRight();

        kb = InputSystem.GetDevice<Keyboard>();
        gettransform = GetComponent<Transform>();
    }

    private void Update()
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

    private void CheckSwingLeft()
    {
        gettransform.transform.position = new Vector3(gettransform.position.x - 1, gettransform.position.y, gettransform.position.z);
        Debug.Log("LEFTPOLEAS");
    }

    private void CheckSwingRight()
    {
        gettransform.transform.position = new Vector3(gettransform.position.x + 1, gettransform.position.y, gettransform.position.z);
        Debug.Log("RIGht P{LEASE");
    }

    private void OnEnable()
    {
        actionSwingLeft.Enable();
        actionSwingRight.Enable();
    }

    private void OnDisable()
    {
        actionSwingLeft.Disable();
        actionSwingRight.Disable();
    }
}