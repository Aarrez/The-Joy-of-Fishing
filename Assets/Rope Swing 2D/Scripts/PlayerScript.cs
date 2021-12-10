using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


//auxiliar script
public class PlayerScript : MonoBehaviour {

	Rigidbody2D rb;
	public float forcetoAdd=100;
	bool keypress;
	public bool player;
	public bool hook;
	bool engaged;
	GameObject CAPS;
	Transform CAPStransform;
	//public InputAction wasdDoubleD;

	void Start () {

		//assigns rigidbody
		rb=GetComponent<Rigidbody2D> ();

		//gives it force an upward forces
		//rb.velocity = Vector2.up * 10;
		CAPS = GameObject.Find("CAPS");
		CAPStransform = CAPS.GetComponent<Transform>();
	}


////////////////////////////////////////////////////////////if (this is player == true){do different controls), or create another script.


	void Update () {

		if(hook == true )
        {
			CAPStransform.transform.position = new Vector2(this.transform.position.x, CAPStransform.transform.position.y);
		}

        //applies force to the left or right
        if (player && Input.GetKeyDown(KeyCode.RightArrow))
		{
			rb.velocity = Vector2.right * forcetoAdd;	
		}
		if (player && Input.GetKeyUp(KeyCode.RightArrow))
		{
			rb.velocity = Vector2.zero;
		}





		if (player && Input.GetKeyDown(KeyCode.LeftArrow))
		{
			rb.velocity = Vector2.left * forcetoAdd;
		}
		if (player && Input.GetKeyUp(KeyCode.LeftArrow))
		{
			rb.velocity = Vector2.zero;
		}




		if (!player && Input.GetKeyDown(KeyCode.S))
		{
			rb.velocity = Vector2.down * forcetoAdd;
		}
		if (!player && Input.GetKeyUp(KeyCode.S))
		{
			rb.velocity = Vector2.zero;
		}





		if (!player && Input.GetKeyDown(KeyCode.W))
		{
			rb.velocity = Vector2.up * forcetoAdd;
		}
		if (!player && Input.GetKeyUp(KeyCode.W))
		{
			rb.velocity = Vector2.zero;
		}






		if (!player && Input.GetKeyDown(KeyCode.A))
		{
			rb.velocity = Vector2.left * forcetoAdd;
		}
		if (!player && Input.GetKeyUp(KeyCode.A))
		{
			rb.velocity = Vector2.zero;
		}


		if (!player && Input.GetKeyDown(KeyCode.D))
		{
			rb.velocity = Vector2.right * forcetoAdd;
		}
		if (!player && Input.GetKeyUp(KeyCode.D))
		{
			rb.velocity = Vector2.zero;
		}




		if (player && Input.GetKeyDown(KeyCode.DownArrow))
		{
			rb.velocity = Vector2.down * forcetoAdd;
		}
		if (player && Input.GetKeyUp(KeyCode.DownArrow))
		{
			rb.velocity = Vector2.zero;
		}




		if (!player && Input.GetKeyDown(KeyCode.UpArrow))
		{
			rb.velocity = Vector2.up * forcetoAdd *2;
		}
		if (!player && Input.GetKeyUp(KeyCode.UpArrow))
		{
			rb.velocity = Vector2.zero;
		}


		//if (keypress == true)
		//      {
		//	return;
		//      }
		//      else { key}

		//if (keypress == true)
		//{
		//	return;
		//}
		//if(keypress == false)

	}			
}
