using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStartupScript : MonoBehaviour
{
    public GameObject CreatePauseMenu;
    CreatePauseMenuScript GetC;
    public GameObject PauseCanvas;
    public GameObject CallButton;
    public GameObject GoFishButton;
    public GameObject ControlPanel;
    bool controlPanelBool;
    void Start()
    {
        CreatePauseMenu.SetActive(true);
        GetC = CreatePauseMenu.GetComponent<CreatePauseMenuScript>();
        ControlPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GetC.SetPause == false)
        {
            PauseCanvas.SetActive(false);
        }

        if(GameManager.instance.moveCam == 3)
        {
            CallButton.SetActive(false);
            GoFishButton.SetActive(false);
        }
        else { CallButton.SetActive(true); }

        if(GameManager.instance.moveCam == 1)
        {
            GoFishButton.SetActive(true);
        }

        if(GameManager.instance.moveCam == 2)
        {
            GoFishButton.SetActive(false);
        }
    }

    public void ClickToEnableControlPanel()
    {
        if(controlPanelBool == false)
        {
            EnableControlPanel();
        }
        else if(controlPanelBool == true)
        {
            DisableControlPanel();
        }
    }
    public void EnableControlPanel()
    {
        ControlPanel.SetActive(true);
        controlPanelBool = true;
    }

    public void DisableControlPanel()
    {
        ControlPanel.SetActive(false);
        controlPanelBool = false;
    }
}
