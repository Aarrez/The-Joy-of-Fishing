using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoatSliderScript : MonoBehaviour
{
    Slider getSlider;
    Transform playerTransform;
    Transform hookTransform;
    float playerIniPos;
    float sliderMaxMin;
    public bool thisIsBoatSlider;
    float elapsed;
    BoatScript boatScript;
    bool cache;
    void Start()
    {
        getSlider = GetComponent<Slider>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        //hookTransform = GameObject.FindGameObjectWithTag("Bait").GetComponent<Transform>();
        playerIniPos = playerTransform.position.x;
        getSlider.value = playerIniPos;
        sliderMaxMin = playerIniPos * 5;
        boatScript = FindObjectOfType<BoatScript>();
        cache = false;
        //getSlider.maxValue = playerIniPos + sliderMaxMin;
        //getSlider.minValue = playerIniPos - sliderMaxMin;
    }

    // Update is called once per frame
    void Update()
    {


        if (boatScript.ropeActive == true && cache == false) // HOW TO SAVE VARIABLES OF OBJECTS THAT ARE INSTANTIATED AFTER START WITHOUT IT DOING IT EVERY FRAME
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 0.2f)
            {
                elapsed = elapsed % 0.2f;
                hookTransform = GameObject.Find("Hook(Clone)").GetComponent<Transform>();
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
        if (thisIsBoatSlider == false && cache == true)
        {
            getSlider.value = hookTransform.position.x;
        }
    }
}
