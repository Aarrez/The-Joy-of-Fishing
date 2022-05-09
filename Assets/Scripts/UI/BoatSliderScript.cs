using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BoatSliderScript : MonoBehaviour
{
    public GameObject getSliderObject;
    public GameObject getBackground;
    Slider getSlider;
    Image getSliderImage;
    public GameObject getHandle;
    Image getHandleImage;
    Transform playerTransform;
    Transform hookTransform;
    float playerIniPos;
    float playerIniPosY;
    float sliderMaxMin;
    public bool thisIsBoatSlider;
    float elapsed;
    PlayerScript boatScript;
    bool cache;
    public bool thisIsHookSliderVertical;
    public bool thisIsHookSliderHorizontal;
    public bool thisIsDepthMeter;
    BoatSliderScript callBoatSliderScriptOfHookVertical;


    TextMeshProUGUI DepthMeterText;
    public float DepthValueVert;
    float depthOffset = -11f;
    public float depthMathTotal;
    void Start()
    {
        DepthMeterText = GameObject.Find("DepthMeter").GetComponent<TextMeshProUGUI>();
        if(thisIsHookSliderHorizontal || thisIsHookSliderVertical || thisIsBoatSlider && !thisIsDepthMeter)
        {
            getSlider = getSliderObject.GetComponent<Slider>();
        }

        if(thisIsHookSliderHorizontal || thisIsHookSliderVertical && !thisIsDepthMeter)
        {
            getSliderImage = getBackground.GetComponent<Image>();
            getHandleImage = getHandle.GetComponent<Image>();
        }
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerIniPos = playerTransform.position.x;
        playerIniPosY = playerTransform.position.y;


        boatScript = FindObjectOfType<PlayerScript>();
        cache = false;


        if(thisIsBoatSlider == true || thisIsHookSliderHorizontal == true)
        {
            getSlider.value = playerIniPos;
        }

        if(thisIsHookSliderVertical == true && !thisIsDepthMeter)
        {
            getSlider.value = playerIniPosY;
        }

        callBoatSliderScriptOfHookVertical = GameObject.Find("HookSliderVertical").GetComponent<BoatSliderScript>();
       
    }


    void Update()
    {


        if (boatScript.ropeActive == true && cache == false) // HOW TO SAVE VARIABLES OF OBJECTS THAT ARE INSTANTIATED AFTER START WITHOUT IT DOING IT EVERY FRAME
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 0.2f)
            {
                elapsed = elapsed % 0.2f;
                hookTransform = GameObject.FindGameObjectWithTag("Bait").GetComponent<Transform>();
                cache = true;
            }


        }

        if (boatScript.ropeActive == false)
        {
            cache = false;
        }


        if(thisIsBoatSlider == true)
        {
            getSlider.value = playerTransform.position.x;
        }
        if (thisIsHookSliderHorizontal && cache == true)
        {
            getSliderImage.color = new Color(1, 1, 1, 1);
            getHandleImage.color = new Color(1, 1, 1, 1);
            getSlider.value = hookTransform.position.x;
        }
        else if (thisIsHookSliderHorizontal && cache == false)
        {
            getSliderImage.color = new Color(1, 1, 1, 0);
            getHandleImage.color = new Color(1, 1, 1, 0);
        }

        if (thisIsHookSliderVertical == true && cache == true && !thisIsDepthMeter)
        {
            getSliderImage.color = new Color(1, 1, 1, 1);
            getHandleImage.color = new Color(1, 1, 1, 1);
            getSlider.value = hookTransform.position.y;
            DepthValueVert = getSlider.value;
            
        }else if(thisIsHookSliderVertical == true && cache == false && !thisIsDepthMeter)
        {
            getSliderImage.color = new Color(1, 1, 1, 0);
            getHandleImage.color = new Color(1, 1, 1, 0);
        }

        if(thisIsDepthMeter == true && cache == true)
        {
            depthMathTotal = depthOffset + callBoatSliderScriptOfHookVertical.DepthValueVert;
            DepthMeterText.color = new Color(1, 1, 1, 1);
            DepthMeterText.text = "Depth (meters): " + depthMathTotal.ToString("F1");
            if(depthMathTotal <= -230)
            {
                DepthMeterText.text = "Warning! Depth Limit Reached! Reel Up!";
            }
        }
        else if (thisIsDepthMeter == true && cache == false)
        {
            DepthMeterText.color = new Color(1, 1, 1, 0);
            DepthMeterText.text = "DepthMeter Offline";

        }
       
    }
}
