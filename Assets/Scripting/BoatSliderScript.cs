using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoatSliderScript : MonoBehaviour
{
    Slider getSlider;
    Transform playerTransform;
    float playerIniPos;
    float sliderMaxMin;
    void Start()
    {
        getSlider = GetComponent<Slider>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerIniPos = playerTransform.position.x;
        getSlider.value = playerIniPos;
        sliderMaxMin = playerIniPos * 5;
        //getSlider.maxValue = playerIniPos + sliderMaxMin;
        //getSlider.minValue = playerIniPos - sliderMaxMin;
    }

    // Update is called once per frame
    void Update()
    {
        getSlider.value = playerTransform.position.x;
    }
}
