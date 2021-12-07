using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CreatePauseMenuScript : MonoBehaviour
{
    TheJoyofFishing getKey;
    InputAction actionEscape;
    GameObject pauseCanvas;
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
            Debug.Log("ESCAPE PAUSE");
            pauseCanvas.SetActive(true);
        } else { Time.timeScale = 1; Debug.Log("ESCAPE un-pause"); pauseCanvas.SetActive(false); }

    }

    public void UnPause()
    {
        Time.timeScale = 1;
        Debug.Log("DONE THROUGH BUTTOPN");
        pauseCanvas.SetActive(false);
    }



}


