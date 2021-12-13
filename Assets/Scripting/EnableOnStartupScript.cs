using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStartupScript : MonoBehaviour
{
    public GameObject CreatePauseMenu;
    CreatePauseMenuScript GetC;
    public GameObject PauseCanvas;
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
    }


}
