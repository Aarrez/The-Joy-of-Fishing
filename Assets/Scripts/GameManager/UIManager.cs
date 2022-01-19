using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public sealed class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager instance;
    public CinemachineVirtualCamera CMcam;
    CinemachineFramingTransposer CMcamBody;
    private RadioMusic radioMusic;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    [HideInInspector] public float CMcamOrthoSize;
    public float currentTime = 0f;
    BoatScript boatScript;
    public TextMeshProUGUI Buttontext;
    public Animator ShopUIAnimator;

private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one UIManager");
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    void Start()
    {
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<BoatScript>();
        CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCam == 1)
        {
            CMcam.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize >= 9)
            {
                CMcam.m_Lens.OrthographicSize = 9;
            }

            Buttontext.text = "Call shop";
        }

        else if (moveCam == 2)
        {
            CMcam.m_Lens.OrthographicSize -= Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize <= 5)
            {
                CMcam.m_Lens.OrthographicSize = 5;
            }

            Buttontext.text = "Return to fishing";
        }

        else if (moveCam == 3)
        {
            CMcam.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize >= 9)
            {
                CMcam.m_Lens.OrthographicSize = 9;
            }
        }

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
            if(currentTime >= 3f)
            {
                SceneManager.LoadScene("End Scene");
            }
        }

    }

        public void onButtonnClick()
    {
        if (moveCam == 1) // ShopCamFalse
        {
            ChangeInteger(); // Change to shop cam
            ShopUIAnimator.Play("ShopUIRollIn");
            radioMusic.PlayRadio();
        }
        else if (moveCam == 2) // ShopCamTrue
        {
            ChangeIntegerAgain(); // Change to boat cam
            ShopUIAnimator.Play("ShopUIRollOut");
            radioMusic.StopRadio();
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
        Transform currenthook;
        currenthook = GameObject.Find("Hook(Clone)").GetComponent<Transform>();
            CMcam.Follow = currenthook;
            CMcamBody.m_TrackedObjectOffset.y = 0;
    }
}
