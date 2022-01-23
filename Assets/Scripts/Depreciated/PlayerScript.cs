using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;


//auxiliar script
public class PlayerScript : MonoBehaviour
{

	public Rigidbody2D rb;

    void Start()
	{
		rb = GetComponent<Rigidbody2D>();
    }

}













//=============================OLD CODE=====================================

/*	TheJoyofFishing GetKey;
    CreatePauseMenuScript cPauseScript;
    public float elapsed = 2f;
    float gruggers;
    Vector2 UpRight = (Vector2.up + Vector2.right).normalized;
    Vector2 UpLeft = (Vector2.up + Vector2.left).normalized;
    Vector2 UpUp = Vector2.up.normalized;
    Vector2 inputvector;
    Vector2 inputvector2;

    

    float check_x;
    float check_y;*/

/*    void Start()
	{
        cPauseScript = FindObjectOfType<CreatePauseMenuScript>();
		//assigns rigidbody
		rb = GetComponent<Rigidbody2D>();

		//gives it force an upward forces


        GetKey = new TheJoyofFishing();
        GetKey.Player.Enable();
        callBoatScript = FindObjectOfType<BoatScript>();
    }*/

/*    private void Update()
    {

        gruggers = RopeScript.instance.Nodes.Count;

        if (cPauseScript.SetPause == false)
        {
            if (GameManager.instance.moveCam == 3)
            {
                if (GameManager.instance.MindcontrolActive == true)
                {

                    elapsed -= Time.deltaTime;

                    inputvector = GetKey.Player.MoveBait.ReadValue<Vector2>();

                    inputvector2 = GetKey.Player.RocketBoost.ReadValue<Vector2>();

                    check_x = inputvector.x;
                    check_y = inputvector.y;

                    //if(callBoatScript.currentlyReelingUp == true)
                    //{
                    //    rb.AddForce(new Vector3(0, 1, 0) * forcetoAdd * Time.deltaTime);
                    //}else if(callBoatScript.currentlyReelingUp == false)
                    //{
                    //    return;
                    //}

                    if (check_y <= Mathf.Sqrt(3) / 2 && check_y >= 1 / 2 && inputvector != Vector2.zero)
                    {
                        if (check_x >= 1 / 2 && check_x <= Mathf.Sqrt(3) / 2) //right joystick up-right ^>
                        {
                            if (inputvector2 == Vector2.up && elapsed <= 0f)
                            {
                                RocketBoostUpAndDir();
                            }

                        }
                        if (check_x <= -1 / 2 && check_x >= -Mathf.Sqrt(3) / 2) //right joystick up-left <^
                        {
                            if (inputvector2 == Vector2.up && elapsed <= 0f)
                            {
                                RocketBoostUpAndDir();

                            }

                        }
                    }

                    if (check_y >= -Mathf.Sqrt(3) / 2 && check_y <= -1 / 2 && inputvector != Vector2.zero)
                    {
                        if (check_x >= 1 / 2 && check_x <= Mathf.Sqrt(3) / 2) //right joystick Down-right ^>
                        {
                            if (inputvector2 == Vector2.up && elapsed <= 0f)
                            {
                                RocketBoostDownAndDir();
                            }

                        }
                        if (check_x <= -1 / 2 && check_x >= -Mathf.Sqrt(3) / 2) //right joystick Down-left <^
                        {
                            if (inputvector2 == Vector2.up && elapsed <= 0f)
                            {
                                RocketBoostDownAndDir();

                            }

                        }
                    }

                    if (check_y >= Mathf.Sqrt(3) / 2 && inputvector != Vector2.zero) //right Joystick straight up
                    {
                        if (inputvector2 == Vector2.up && elapsed <= 0f)
                        {
                            RocketBoostUp();
                        }

                    }

                    if (check_y <= -Mathf.Sqrt(3) / 2 && inputvector != Vector2.zero) //right Joystick straight Down
                    {
                        if (inputvector2 == Vector2.up && elapsed <= 0f)
                        {
                            RocketBoostUp();
                        }

                    }

                    
                    if(check_x >= Mathf.Sqrt(3) / 2 && inputvector != Vector2.zero) //right Joystick straight RIGHT
                    {
                        if (inputvector2 == Vector2.up && elapsed <= 0f)
                        {
                            RocketBoostDir();
                        }
                    }

                    if (check_x <= -Mathf.Sqrt(3) / 2 && inputvector != Vector2.zero) //right Joystick straight Left
                    {
                        if (inputvector2 == Vector2.up && elapsed <= 0f)
                        {
                            RocketBoostDir();
                        }
                    }

                    if (elapsed <= 0f)
                    {
                        elapsed = 0f;
                    }
                }
            }




        }
    }
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd * Time.deltaTime);
    }
    void RocketBoostUp()
    {
        if(gruggers < 100)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 200, ForceMode2D.Impulse);
        }

        if (gruggers > 100 && gruggers < 200)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 100, ForceMode2D.Impulse);
        }

        if (gruggers > 200 && gruggers <= 700)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 75, ForceMode2D.Impulse);
        }
        elapsed = 3f;
    }

    void RocketBoostDown()
    {
        if (gruggers < 100)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 200, ForceMode2D.Impulse);
        }

        if (gruggers > 100 && gruggers < 200)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 100, ForceMode2D.Impulse);
        }

        if (gruggers > 200 && gruggers <= 700)
        {
            rb.AddForce(new Vector3(0, inputvector.y, 0) * forcetoAdd / 75, ForceMode2D.Impulse);
        }
        elapsed = 3f;
    }

    void RocketBoostUpAndDir()
    {
        if (gruggers < 100)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 200, ForceMode2D.Impulse);
        }

        if (gruggers > 100 && gruggers < 200)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 100, ForceMode2D.Impulse);
        }

        if (gruggers > 200 && gruggers <= 700)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 75, ForceMode2D.Impulse);
        }

        elapsed = 3f;
    }



    void RocketBoostDownAndDir()
    {
        if (gruggers < 100)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 200, ForceMode2D.Impulse);
        }

        if (gruggers > 100 && gruggers < 200)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 100, ForceMode2D.Impulse);
        }

        if (gruggers > 200 && gruggers <= 700)
        {
            rb.AddForce(new Vector3(inputvector.x, inputvector.y, 0) * forcetoAdd / 75, ForceMode2D.Impulse);
        }

        elapsed = 3f;
    }

    void RocketBoostDir()
    {
        if (gruggers < 100)
        {
            rb.AddForce(new Vector3(inputvector.x, 0, 0) * forcetoAdd / 200, ForceMode2D.Impulse);
        }

        if (gruggers > 100 && gruggers < 200)
        {
            rb.AddForce(new Vector3(inputvector.x, 0, 0) * forcetoAdd / 100, ForceMode2D.Impulse);
        }

        if (gruggers > 200 && gruggers <= 700)
        {
            rb.AddForce(new Vector3(inputvector.x, 0, 0) * forcetoAdd / 75, ForceMode2D.Impulse);
        }

        elapsed = 3f;
    }*/