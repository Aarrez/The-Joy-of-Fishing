using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    [SerializeField] private bool hookSubmerged = false; //Hook is under the watersurface

    private FMOD.Studio.EventInstance splashEvent;
    private BoatEmitter boatEmitter;

    //water ambience MOVE ME TO BETTER PLACE LATER?
    private FMOD.Studio.EventInstance lakeAmbienceEvent;

    private void Awake()
    {
        boatEmitter = FindObjectOfType<BoatEmitter>();

        splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        splashEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/hook_splash");
        splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //splashEvent.start();

        lakeAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/ambience_lake");
        lakeAmbienceEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(boatEmitter.transform.position));
        lakeAmbienceEvent.start();
    }

    private void Update()
    {
        if (hookSubmerged)
        {
            lakeAmbienceEvent.setParameterByName("music_duck", 1);
            boatEmitter.UnderWater();
        }
        else
        {
            lakeAmbienceEvent.setParameterByName("music_duck", 0); //not paused
            boatEmitter.AboveWater();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //WaterSurface need to be BaitLayer. This ok?
    {
        if (other.gameObject.CompareTag("Bait"))
        {
            if (other.transform.position.y > gameObject.transform.position.y)
            {
                hookSubmerged = true;


                //Play FMOD Splash if not already playing to avoid duplicates
                FMOD.Studio.PLAYBACK_STATE pbState;
                splashEvent.getPlaybackState(out pbState);
                if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.gameObject));
                    splashEvent.start();

                    // No need to release this from memory. It is 1 sound played once at times.
                }
            }
            else if (other.transform.position.y < gameObject.transform.position.y)
            {
                hookSubmerged = false;
            }
        }
    }
}