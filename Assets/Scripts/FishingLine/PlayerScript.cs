using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


//auxiliar script
public class PlayerScript : MonoBehaviour
{

	Rigidbody2D rb;
	public float forcetoAdd = 100;
	TheJoyofFishing GetKey;
	InputAction Move;
	InputAction MoveBait;
    Vector2 move;
    bool buttonhold;
    bool moveOn;

	void Start()
	{

		//assigns rigidbody
		rb = GetComponent<Rigidbody2D>();

		//gives it force an upward forces
		rb.velocity = Vector2.up * 10;
		GetKey = new TheJoyofFishing();
		Move = GetKey.Player.Move;
		MoveBait = GetKey.Player.MoveBait;
		GetKey.Player.Enable();
        //GetKey.Player.MoveBait.started += Mannen;
        //GetKey.Player.MoveBait.performed += Mannen;
        //GetKey.Player.MoveBait.canceled += Mannen;
        //GetKey.Player.MoveBait.canceled += Mannen2;

    }

	//public void Mannen(InputAction.CallbackContext ctx)
	//{
 //       move = GetKey.Player.MoveBait.ReadValue<Vector2>();
	//	//Debug.Log(move.x * forcetoAdd);
		

 //           buttonhold = !buttonhold;
	//}

    //public void Mannen2(InputAction.CallbackContext ctx)
    //{
    //    //Vector2 move = GetKey.Player.MoveBait.ReadValue<Vector2>();
    //    ////Debug.Log(move.x * forcetoAdd);
    //    //rb.AddForce(new Vector2(move.x * forcetoAdd, move.y * forcetoAdd));
    //    buttonhold = false;
    //    return;

    //}

    void Update()
    {
        if(moveOn == true)
        {
            move = GetKey.Player.MoveBait.ReadValue<Vector2>();
            buttonhold = true;
        }

        if (buttonhold == true)
        {
            rb.AddForce(new Vector2(move.x * forcetoAdd, move.y * forcetoAdd));
            Debug.Log(move);

        }
        else if(buttonhold == false){
            rb.AddForce(Vector2.zero);
        }
        Debug.Log(buttonhold);
    }


    //	void OnEnable()
    //    {
    //		GetKey.Enable();
    //		Move.Enable();
    //		MoveBait.Enable();

    //		Move.performed += _ => CheckMove();
    //		MoveBait.performed += _ => CheckMoveBait();
    //	}
    //	void OnDisable()
    //    {
    //		GetKey.Disable();
    //		Move.Disable();
    //		MoveBait.Disable();
    //	}

    //		//CheckMove();
    //		CheckMoveBait();

    //		//Vector2 movegrunk = GetKey.Player.MoveBait.ReadValue<Vector2>();
    //		//Debug.Log(movegrunk);
    //	}			

    //	void CheckMove()
    //    {

    //    }

    //	void CheckMoveBait()
    //	{
    //		rb.AddRelativeForce(new Vector2(0, 0) * forcetoAdd);
    //		Debug.Log("CURRENT move");
    //	}
    //}
}
