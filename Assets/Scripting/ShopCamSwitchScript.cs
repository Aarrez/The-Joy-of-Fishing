using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class ShopCamSwitchScript : MonoBehaviour
{
    TextMeshProUGUI Buttontext;
    

    // Start is called before the first frame update
    void Start()
    {
        Buttontext = GameObject.Find("CallButtonText").GetComponent<TextMeshProUGUI>();

    }
    public void onButtonnClick()
    {
        if (GameManager.instance.moveCam == 1)
        {
            GameManager.instance.ChangeInteger();
        }
        else if (GameManager.instance.moveCam == 2)
        { 
            GameManager.instance.ChangeIntegerAgain(); 
        }
    }


}
