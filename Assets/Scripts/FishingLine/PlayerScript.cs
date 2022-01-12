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
    public float elapsed = 4f;
    float gruggers;
    Vector2 UpRight = (Vector2.up + Vector2.right).normalized;
    Vector2 UpLeft = (Vector2.up + Vector2.left).normalized;
    Vector2 UpUp = Vector2.up.normalized;
    Vector2 inputvector;
    Vector2 inputvector2;

    float check_x;
    float check_y;

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
 
        gruggers = RopeScript.instance.Nodes.Count;

        if (cPauseScript.SetPause == false)
        {
            if(GameManager.instance.moveCam == 3)
            {

               elapsed += Time.deltaTime;

                Debug.Log(elapsed);
                inputvector = GetKey.Player.MoveBait.ReadValue<Vector2>();
                rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd * Time.deltaTime);
                inputvector2 = GetKey.Player.RocketBoost.ReadValue<Vector2>();

                check_x = inputvector.x;
                check_y = inputvector.y;

                if (check_y <= Mathf.Sqrt(3) / 2 && check_y >= 1 / 2 && inputvector != Vector2.zero)
                {
                    if (check_x >= 1 / 2 && check_x <= Mathf.Sqrt(3) / 2) //right joystick up-right ^>
                    {
                        if (inputvector2 == Vector2.up && elapsed >= 5f)
                        {
                            RocketBoostUpAndDir();
                        }

                    }
                    if (check_x <= -1 / 2 && check_x >= -Mathf.Sqrt(3) / 2) //right joystick up-left <^
                    {
                        if (inputvector2 == Vector2.up && elapsed >= 5f)
                        {
                            RocketBoostUpAndDir();

                        }

                    }
                }

                if (check_y >= Mathf.Sqrt(3) / 2 && inputvector != Vector2.zero) //right Joystick straight up
                {
                    if (inputvector2 == Vector2.up && elapsed >= 5f)
                    {
                        RocketBoostUp();
                    }

                }

            }
            if (elapsed >= 5f)
            {
                elapsed = 5f;
            }
        }
           
        


    }

    void RocketBoostUp()
    {
        rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd * (gruggers / 2) * Time.deltaTime);
        elapsed = elapsed % 5f;
    }

    void RocketBoostUpAndDir()
    {
        rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd * (gruggers / 2) * Time.deltaTime);
        elapsed = elapsed % 5f;
    }
    
}
