using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;


//auxiliar script
public class PlayerScript : MonoBehaviour
{

	Rigidbody2D rb;
	public float forcetoAdd = 100;
	TheJoyofFishing GetKey;

	void Start()
	{

		//assigns rigidbody
		rb = GetComponent<Rigidbody2D>();

		//gives it force an upward forces


        GetKey = new TheJoyofFishing();
        GetKey.Player.Enable();
        
    }

   

    private void Update()
    {
        
        Vector2 inputvector = GetKey.Player.MoveBait.ReadValue<Vector2>();
        rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd);
    }
    
}
