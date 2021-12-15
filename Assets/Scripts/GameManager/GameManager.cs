using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera CMcam;
    CinemachineFramingTransposer CMcamBody;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    BoatScript boatScript;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one Gamemanager");
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<BoatScript>();
        CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();


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
        CMcamBody.m_TrackedObjectOffset.y = 3;
    }

    public void ShopCamFalse()
    {
        CMcam.Follow = Player;
        CMcamBody.m_TrackedObjectOffset.y = 3;
    }

    public void BaitCam()
    {
        if (RopeScript.instance.grub == true)
        {
            CMcam.Follow = RopeScript.instance.go.transform;
            CMcamBody.m_TrackedObjectOffset.y = 0;
        }

    }

}
