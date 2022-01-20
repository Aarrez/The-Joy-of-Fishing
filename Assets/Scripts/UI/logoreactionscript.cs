using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class logoreactionscript : MonoBehaviour
{
    public Rigidbody2D logo;
    public GameObject canvas;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.moveCam == 3 || GameManager.instance.moveCam == 2)
        {
            StartCoroutine("ActiveLogo");
        }
        /*else if(Gamepad.current.xButton.IsPressed())
        {
            StartCoroutine("ActiveLogo");
        }*/
        //else if(Mouse.current.leftButton.IsPressed())
        //{
        //    StartCoroutine("ActiveLogo");
        //}
    }

    IEnumerator ActiveLogo()
    {
        logo.simulated = true;
        yield return new WaitForSecondsRealtime(5f);
        Object.Destroy(canvas);
    }
}
