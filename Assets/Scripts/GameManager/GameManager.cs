using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera CMcam;
    public int moveCam = 1;
    public bool baitCam;
    Transform ShoppeBoat;
    Transform Player;
    private void Awake() 
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCam == 2)
        {
            ShopCamTrue();
        }

        if (moveCam == 1)
        {
            ShopCamFalse();
        }

        if (moveCam == 3)
        {

        }

        //if (moveCamToShop == 0)
        //{
        //    CMcam.Follow = Hook;
        //}
    }
    public void ChangeInteger()
    {
        moveCam = 2;
    }

    public void ChangeIntegerAgain()
    {
        moveCam = 1;
    }

    public void ShopCamTrue()
    {
        CMcam.Follow = ShoppeBoat;
    }

    public void ShopCamFalse()
    {
        CMcam.Follow = Player;
    }

}
