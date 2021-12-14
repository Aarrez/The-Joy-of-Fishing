using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera CMcam;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    private void Awake() 
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCam == 2 && baitCam == false)
        {
            ShopCamTrue();
        }

        if (moveCam == 1 && baitCam == false)
        {
            ShopCamFalse();
        }

        if (moveCam == 3 && baitCam == true)
        {
            BaitCam();
        }

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

    public void BaitCam()
    {
        CMcam.Follow = Hook;
    }

}
