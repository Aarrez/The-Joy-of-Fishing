using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;



public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    CinemachineFramingTransposer CMcamBody;
    public RadioMusic radioMusic;
    
    [Header("UI Control Center")]
    public CinemachineVirtualCamera CMcam;
    public GameObject pauseCanvas, callShopCanvas, goFishCanvas;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam = false;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    [HideInInspector] public float CMcamOrthoSize;
    public TextMeshProUGUI Buttontext;
    public Animator ShopUIAnimator;
    
    BoatScript boatScript;

    [HideInInspector] public bool MindcontrolActive = false;
    public int currentLineLevel = 0, currentBait = 0, cashAmount = 0;
    public float currentTime = 0f;
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
        Fadeimage.color = new Color(0, 0, 0, 255);
    }
    // Start is called before the first frame update
    void Start()
    {
        UIScreenfadein();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<BoatScript>();
        CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (moveCam == 1)
        {
            callShopCanvas.SetActive(true);
            goFishCanvas.SetActive(true);
            CMcam.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize >= 9)
            {
                CMcam.m_Lens.OrthographicSize = 9;
            }
            CMcam.Follow = Player;
            CMcamBody.m_TrackedObjectOffset.y = 3;
            Buttontext.text = "Call shop";
        }

        else if (moveCam == 2)
        {
            callShopCanvas.SetActive(true);
            goFishCanvas.SetActive(false);
            CMcam.m_Lens.OrthographicSize -= Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize <= 5)
            {
                CMcam.m_Lens.OrthographicSize = 5;
            }
            CMcam.Follow = ShoppeBoat;
            CMcamBody.m_TrackedObjectOffset.y = 3;
            Buttontext.text = "Return to fishing";
        }

        else if (moveCam == 3)
        {
            callShopCanvas.SetActive(false);
            goFishCanvas.SetActive(false);
            CMcam.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (CMcam.m_Lens.OrthographicSize >= 9)
            {
                CMcam.m_Lens.OrthographicSize = 9;
            }
            CMcam.Follow = BaitCam();
            CMcamBody.m_TrackedObjectOffset.y = 0;
            if(currentTime >= 3f)
            {
                SceneManager.LoadScene("End Scene");
            }
        }
    }

    public void onShopSwitch()
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
    public Transform BaitCam()
    {
        Transform currenthook;
        currenthook = GameObject.FindGameObjectWithTag("Bait").GetComponent<Transform>();
        return currenthook;
    }


//============================ ScreenFader ============================
    public Image Fadeimage;
    public GameObject FadeCanvas;
    public void UIScreenfadeout() 
    {
        StartCoroutine(FadeOutCR());
    }
    public void UIScreenfadein() 
    {
        StartCoroutine(FadeInCR());

    } 
    private IEnumerator FadeOutCR()
    {
        float duration = 4f; //0.5 secs
        currentTime = 0f;
        FadeCanvas.SetActive(true);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.MoveTowards(0f, 1f, currentTime/duration);
            Fadeimage.color = new Color(Fadeimage.color.r, Fadeimage.color.g, Fadeimage.color.b, alpha);
        }
        yield break;

    }

    private IEnumerator FadeInCR()
    {
        float duration = 2f; //0.5 secs
        float currentTime = 0f;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.MoveTowards(1f, 0f, currentTime/duration);
            Fadeimage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        FadeCanvas.SetActive(false);
        yield break;
    }

}