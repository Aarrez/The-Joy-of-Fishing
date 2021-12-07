using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CreatePauseMenuScript : MonoBehaviour
{
    TheJoyofFishing getKey;
    InputAction actionEscape;
    void Awake()
    {
        getKey = new TheJoyofFishing();
        actionEscape = getKey.EscapeToPause.Pause;
    }








    void OnEnable()
    {
        actionEscape.Enable();
        actionEscape.performed += _ => Pause();
        actionEscape.Enable();

    }

    void OnDisable()
    {
        actionEscape.Disable();
        actionEscape.Enable();
    }
    void Pause()
    {
        Debug.Log("escape!!!");
    }

}
