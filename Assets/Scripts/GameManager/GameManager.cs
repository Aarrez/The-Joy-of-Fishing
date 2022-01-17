using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera CMcam;
    CinemachineFramingTransposer CMcamBody;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    BoatScript boatScript;

    public int currentLineLevel = 0, currentBait = 0;
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
        //CMcam = GetComponent<CinemachineVirtualCamera>();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<BoatScript>();
        CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        UIScreenfadein();

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
            if(currentTime >= 3f)
            {
                SceneManager.LoadScene("End Scene");
            }
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
        float duration = 3f; //0.5 secs
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
        float duration = 1f; //0.5 secs
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
