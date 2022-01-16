using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class ShopCamSwitchScript : MonoBehaviour
{
    TextMeshProUGUI Buttontext;
    CinemachineVirtualCamera cmVirt;
    float cmVirtOrthoSize;
    private RadioMusic radioMusic;


    // Start is called before the first frame update
    void Start()
    {
        Buttontext = GameObject.Find("CallButtonText").GetComponent<TextMeshProUGUI>();
        Buttontext.text = "Go to shop";
        cmVirt = GetComponent<CinemachineVirtualCamera>();

        radioMusic = FindObjectOfType<RadioMusic>();
    }

    public void onButtonnClick()
    {
        if (GameManager.instance.moveCam == 1) // ShopCamFalse
        {
            GameManager.instance.ChangeInteger(); // Change to shop cam
            radioMusic.PlayRadio();
        }
        else if (GameManager.instance.moveCam == 2) // ShopCamTrue
        {
            GameManager.instance.ChangeIntegerAgain(); // Change to boat cam
            radioMusic.StopRadio();
        }
    }

    void Update()
    {
        if (GameManager.instance.moveCam == 1)
        {
            cmVirt.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (cmVirt.m_Lens.OrthographicSize >= 9)
            {
                cmVirt.m_Lens.OrthographicSize = 9;
            }

            Buttontext.text = "Call shop";
        }

        if (GameManager.instance.moveCam == 2)
        {
            cmVirt.m_Lens.OrthographicSize -= Time.deltaTime * 3;
            if (cmVirt.m_Lens.OrthographicSize <= 5)
            {
                cmVirt.m_Lens.OrthographicSize = 5;
            }

            Buttontext.text = "Return to fishing";
        }

        if (GameManager.instance.moveCam == 3)
        {
            cmVirt.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (cmVirt.m_Lens.OrthographicSize >= 9)
            {
                cmVirt.m_Lens.OrthographicSize = 9;
            }
        }
    }
}