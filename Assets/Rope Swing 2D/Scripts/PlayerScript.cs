using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


//auxiliar script
public class PlayerScript : MonoBehaviour {

	Rigidbody2D rb;
	public float forcetoAdd=100;
	//public InputAction wasdDoubleD;

	void Start () {

		//assigns rigidbody
		rb=GetComponent<Rigidbody2D> ();

		//gives it force an upward forces
		//rb.velocity = Vector2.up * 10;

	}


////////////////////////////////////////////////////////////if (this is player == true){do different controls), or create another script.


	void Update () {

        //applies force to the left or right
        if (Input.GetKey(KeyCode.RightArrow))
		{
			rb.velocity = Vector2.right * forcetoAdd;

		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rb.velocity = Vector2.left * forcetoAdd;

		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			rb.velocity = Vector2.down * forcetoAdd;

		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			rb.velocity = Vector2.up * forcetoAdd;

		}

	}			
}
