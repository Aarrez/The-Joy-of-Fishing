using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class ShopCamSwitchScript : MonoBehaviour
{
    CinemachineVirtualCamera CMcam;
    int moveCamToShop = 1;
    Transform ShoppeBoat;
    Transform Player;
    //Transform Hook;
    TextMeshProUGUI Buttontext;
    

    // Start is called before the first frame update
    void Start()
    {
        CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        //Hook = GameObject.Find("Hook").GetComponent<Transform>();
        Buttontext = GameObject.Find("CallButtonText").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if (moveCamToShop == 2)
        {
            ShopCamTrue();
        }

        if (moveCamToShop == 1)
        {
            ShopCamFalse();
        }

        //if (moveCamToShop == 0)
        //{
        //    CMcam.Follow = Hook;
        //}
    }
    public void ChangeInteger()
    {
        moveCamToShop = 2;
    }

    public void ChangeIntegerAgain()
    {
        moveCamToShop = 1;
    }

    public void onButtonnClick()
    {
        if (moveCamToShop == 1)
        {
            ChangeInteger(); ;
        }
        else { ChangeIntegerAgain(); }
    }

    void ShopCamTrue()
    {
        CMcam.Follow = ShoppeBoat;
        Buttontext.text = "Return to fishing";
    }

    void ShopCamFalse()
    {
        CMcam.Follow = Player;
        Buttontext.text = "Go to shop";
    }

}
