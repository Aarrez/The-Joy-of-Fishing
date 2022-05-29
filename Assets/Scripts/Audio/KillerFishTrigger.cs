using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class KillerFishTrigger : MonoBehaviour
{
    /*private FMOD.Studio.EventInstance statueEvent;
    private bool triggered = false;
    private Light2D[] eyeLight;
    private float lightValue;
    private float changePerSecond = 0.1f;
    private bool goingUp = true;
    [SerializeField] private bool done = false;

    private void Awake()
    {
        statueEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/killerfish_statue");
        eyeLight = GetComponentsInChildren<Light2D>();
        statueEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    private void Update()
    {
        if (triggered && !done) // Gradually change eye light up and down
        {
            foreach (Light2D light in eyeLight)
            {
                if (goingUp)
                {
                    light.intensity += changePerSecond * Time.deltaTime;
                    if (light.intensity > 0.8)
                    {
                        goingUp = false;
                    }
                }
                else
                {
                    light.intensity -= changePerSecond * Time.deltaTime;
                    if (light.intensity < 0)
                    {
                        done = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bait") && !triggered)
        {
            Debug.Log("BAIT ENTERED " + gameObject);
            triggered = true;
            statueEvent.start();
            statueEvent.release();
        }
    }*/
}