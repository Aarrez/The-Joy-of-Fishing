using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableOnStartupScript : MonoBehaviour
{
    public GameObject CreatePauseMenu;
    CreatePauseMenuScript GetC;
    public GameObject PauseCanvas;
    public GameObject CallButton;
    public GameObject GoFishButton;
    public GameObject ControlPanel;
    public GameObject CoolDownUIText;
    TextMeshProUGUI CoolDownText;
    bool controlPanelBool;
    PlayerScript callPlayerScript;
    bool cache;
    float cacheTimeElapsed;
    void Start()
    {
        CreatePauseMenu.SetActive(true);
        GetC = CreatePauseMenu.GetComponent<CreatePauseMenuScript>();
        ControlPanel.SetActive(false);
        CoolDownText = CoolDownUIText.GetComponent<TextMeshProUGUI>();
        CoolDownUIText.SetActive(false);

        cache = false;
    }

    // Update is called once per frame
    void Update()
    {

       
        if(GetC.SetPause == false)
        {
            PauseCanvas.SetActive(false);
        }

        if(GameManager.instance.moveCam == 3) //cam following fishing hook, underwater
        {
            CallButton.SetActive(false);
            GoFishButton.SetActive(false);
            CoolDownUIText.SetActive(true);

            if (cache == false)
            {
                cacheTimeElapsed += Time.deltaTime;
                if (cacheTimeElapsed >= 0.1f)
                {
                    cacheTimeElapsed = cacheTimeElapsed % 0.1f;
                    callPlayerScript = FindObjectOfType<PlayerScript>();
                    cache = true;
                    Debug.Log("CHACHE");
                }
            }

            CoolDownText.text = "RocketBoost Cooldown: " + callPlayerScript.elapsed;
        }
        else { CallButton.SetActive(true); }

        if(GameManager.instance.moveCam == 1) //cam following player, overwater
        {
            GoFishButton.SetActive(true);
        }

        if(GameManager.instance.moveCam == 2) //cam following shop
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
