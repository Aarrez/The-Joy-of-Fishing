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
    CreatePauseMenuScript cPauseScript;
    float elapsed = 4f;
    float gruggers;

    //RopeScript callRopeScript;

	void Start()
	{
        cPauseScript = FindObjectOfType<CreatePauseMenuScript>();
		//assigns rigidbody
		rb = GetComponent<Rigidbody2D>();

		//gives it force an upward forces


        GetKey = new TheJoyofFishing();
        GetKey.Player.Enable();
       
    }

   

    private void Update()
    {
        elapsed += Time.deltaTime;
        gruggers = RopeScript.instance.Nodes.Count;

        if (cPauseScript.SetPause == false)
        {
            Vector2 inputvector = GetKey.Player.MoveBait.ReadValue<Vector2>();
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd * Time.deltaTime);

            Vector2 inputvector2 = GetKey.Player.RocketBoost.ReadValue<Vector2>();
            //if (inputvector2 == Vector2.up && elapsed >= 5f)
            //{
            //    rb.AddForce(new Vector3(0, inputvector2.y, 0) * forcetoAdd * gruggers * Time.deltaTime);
            //    elapsed = elapsed % 5f;
            //}
            if(inputvector == Vector2.)
            {
                Debug.Log("MEMES2");
                ////if(inputvector2 == Vector2.up && elapsed >= 5f)
                ////{
                ////    rb.AddForce(new Vector3(0, inputvector2.y, 0) * forcetoAdd * gruggers * Time.deltaTime);
                ////    elapsed = elapsed % 5f;
                ////}

            }

            ////if (inputvector2 == Vector2.up && elapsed >= 5f && inputvector == Vector2.left && inputvector == Vector2.up)
            ////{
            ////    rb.AddForce(new Vector3(-inputvector2.x, 0, 0) * forcetoAdd * gruggers * Time.deltaTime);
            ////    elapsed = elapsed % 5f;
            ////}
        }
        if (elapsed >= 5f)
        {
            elapsed = 5f;
        }
        Debug.Log(elapsed + "   hello   " + gruggers);


    }
    
}
