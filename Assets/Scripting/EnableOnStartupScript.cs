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
    [SerializeField] GameObject CoolDownUIText;
    public GameObject NodesCountUI;
    TextMeshProUGUI NodesCountText;
    TextMeshProUGUI CoolDownText;
    bool controlPanelBool;
    PlayerScript callPlayerScript;
    bool cache;
    float cacheTimeElapsed;

    BoatScript callBoatScript;
    void Start()
    {
        CreatePauseMenu.SetActive(true);
        GetC = CreatePauseMenu.GetComponent<CreatePauseMenuScript>();
        ControlPanel.SetActive(false);
        CoolDownText = CoolDownUIText.GetComponent<TextMeshProUGUI>();
        NodesCountText = NodesCountUI.GetComponent<TextMeshProUGUI>();
        NodesCountUI.SetActive(false);
        callBoatScript = FindObjectOfType<BoatScript>();

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
            CoolDownText.color = new Color(1, 1, 1, 1);
            NodesCountUI.SetActive(true);

            if (cache == false)
            {
                cacheTimeElapsed += Time.deltaTime;
                if (cacheTimeElapsed >= 0.5f)
                {
                    cacheTimeElapsed = cacheTimeElapsed % 0.5f;
                    callPlayerScript = FindObjectOfType<PlayerScript>();
                    cache = true;
                }
            }

            if(cache == true && !callPlayerScript)
            {

                cacheTimeElapsed += Time.deltaTime;
                if (cacheTimeElapsed >= 0.5f)
                {
                    callPlayerScript = FindObjectOfType<PlayerScript>();
                }
            }

            
            if(RopeScript.instance.Nodes.Count >= callBoatScript.maxLineLength)
            {
                NodesCountText.text = "Line Length (feet): MAX" + "(" + RopeScript.instance.Nodes.Count + ")";
            }
            else 
            { 
                NodesCountText.text = "Line Length (feet): " + RopeScript.instance.Nodes.Count; 
            }


            if(callPlayerScript && callPlayerScript.elapsed <= 0f)
            {
                CoolDownText.text = "RocketBoost: Ready";
            }else if (callPlayerScript && callPlayerScript.elapsed <= 5)
            {
                CoolDownText.text = "RocketBoost: Cooldown " + callPlayerScript.elapsed.ToString("F1");
            }


        }
        else { CallButton.SetActive(true); NodesCountUI.SetActive(false); CoolDownText.color = new Color(1, 1, 1, 0);

            if (!callPlayerScript)
            {
                CoolDownText.text = "RocketBoost Offline";
            }

            

        }




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
