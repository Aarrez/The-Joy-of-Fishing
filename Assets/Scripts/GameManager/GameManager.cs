using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
using Stem;



public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [Header("UI Control Center")]
    public GameObject pauseCanvas, callShopCanvas, goFishCanvas;
    [HideInInspector] public int moveCam = 1;
    [HideInInspector] public bool baitCam = false;
    [HideInInspector] public Transform ShoppeBoat, Player, Hook;
    [HideInInspector] public float CMcamOrthoSize;
    public Animator ShopUIAnimator;
    PlayerScript boatScript;

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
    void Start()
    {
        UIScreenfadein();
        ShoppeBoat = GameObject.Find("ShoppeBoat").GetComponent<Transform>();
        Player = GameObject.Find("Player").GetComponent<Transform>();
        boatScript = FindObjectOfType<PlayerScript>();
        //CMcamBody = CMcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void onShopSwitch()
    {
        if (moveCam == 1) // ShopCamFalse
        {
            ChangeInteger(); // Change to shop cam
            ShopUIAnimator.Play("ShopUIRollIn");
            //radioMusic.PlayRadio();
        }
        else if (moveCam == 2) // ShopCamTrue
        {
            ChangeIntegerAgain(); // Change to boat cam
            ShopUIAnimator.Play("ShopUIRollOut");
            //radioMusic.StopRadio();
        }
    }

    public void ChangeInteger()
    {
        moveCam = 2;
        ProCamera2D.Instance.RemoveAllCameraTargets();
        ProCamera2D.Instance.AddCameraTarget(ShoppeBoat.transform, 1f, 1f, 0f);
    }

    public void ChangeIntegerAgain()
    {
        moveCam = 1;
        ProCamera2D.Instance.RemoveAllCameraTargets();
        ProCamera2D.Instance.AddCameraTarget(Player.transform, 1f, 1f, 0f);
    }

//============================ PauseScreen ============================
    public bool SetPause;
    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            SetPause = true;
        } 
        else 
        {
            Time.timeScale = 1; pauseCanvas.SetActive(false);
            SetPause = false; 
        }

    }

    /*public void UnPause()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        SetPause = false;
    }*/

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