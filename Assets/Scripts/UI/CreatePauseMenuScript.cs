using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CreatePauseMenuScript : MonoBehaviour
{
    TheJoyofFishing getKey;
    InputAction actionEscape;
    GameObject pauseCanvas;
    public bool SetPause;
    void Awake()
    {
        getKey = new TheJoyofFishing();
        actionEscape = getKey.EscapeToPause.Pause;
        pauseCanvas = GameObject.Find("PauseCanvas");
        pauseCanvas.SetActive(false);
    }








    void OnEnable()
    {
        actionEscape.Enable();
        actionEscape.performed += _ => Pause();

    }

    void OnDisable()
    {
        actionEscape.Disable();
    }
    void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            SetPause = true;
        } else { Time.timeScale = 1; pauseCanvas.SetActive(false); SetPause = false; }

    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        SetPause = false;
    }



}


