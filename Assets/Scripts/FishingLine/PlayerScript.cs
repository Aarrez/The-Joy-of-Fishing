using UnityEngine;
using System.Collections;


//auxiliar script
public class PlayerScript : MonoBehaviour {

	Rigidbody2D rb;
	public float forcetoAdd=100;

	void Start () {

		//assigns rigidbody
		rb=GetComponent<Rigidbody2D> ();

		//gives it force an upward forces
		rb.velocity = Vector2.up * 10;

	}


	void Update () {

	 	//applies force to the left or right
		rb.AddForce(Vector2.right*Input.GetAxisRaw ("Horizontal")*forcetoAdd);
	}			
}
