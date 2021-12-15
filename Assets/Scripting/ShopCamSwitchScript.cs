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
    

    // Start is called before the first frame update
    void Start()
    {
        Buttontext = GameObject.Find("CallButtonText").GetComponent<TextMeshProUGUI>();
        Buttontext.text = "Go to shop";
        cmVirt = GetComponent<CinemachineVirtualCamera>();

    }
    public void onButtonnClick()
    {
        if (GameManager.instance.moveCam == 1)
        {
            GameManager.instance.ChangeInteger();
            Buttontext.text = "Return to fishing";



        }
        else if (GameManager.instance.moveCam == 2)
        { 
            GameManager.instance.ChangeIntegerAgain();
            Buttontext.text = "Go to shop";

            
        }
    }

    void Update()
    {
        if(GameManager.instance.moveCam == 1)
        {
            cmVirt.m_Lens.OrthographicSize += Time.deltaTime * 3;
            if (cmVirt.m_Lens.OrthographicSize >= 9)
            {
                cmVirt.m_Lens.OrthographicSize = 9;
            }
        }

        if (GameManager.instance.moveCam == 2)
        {
            cmVirt.m_Lens.OrthographicSize -= Time.deltaTime * 3;
            if (cmVirt.m_Lens.OrthographicSize <= 5)
            {
                cmVirt.m_Lens.OrthographicSize = 5;
            }
        }
    }


}
