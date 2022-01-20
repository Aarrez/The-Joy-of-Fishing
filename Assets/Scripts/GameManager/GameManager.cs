using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public sealed class GameManager : MonoBehaviour
{
    public static GameManager instance = null;


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
