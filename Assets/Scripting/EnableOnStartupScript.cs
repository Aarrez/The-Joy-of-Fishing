using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStartupScript : MonoBehaviour
{
    public GameObject CreatePauseMenu;
    CreatePauseMenuScript GetC;
    public GameObject PauseCanvas;
    public GameObject CallButton;
    void Start()
    {
        CreatePauseMenu.SetActive(true);
        GetC = CreatePauseMenu.GetComponent<CreatePauseMenuScript>();
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
        }
        else { CallButton.SetActive(true); }
    }


}
