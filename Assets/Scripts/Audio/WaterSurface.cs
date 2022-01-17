using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
   [SerializeField] private bool hookSubmerged = false; //Hook is under the watersurface

    private FMOD.Studio.EventInstance splashEvent;
    private BoatSliderScript boatSliderScript;

    private BoatEmitter boatEmitter;
    
    //water ambience MOVE ME TO BETTER PLACE LATER?
    private FMOD.Studio.EventInstance lakeAmbienceEvent;

    private void Awake()
    {
        boatSliderScript = FindObjectOfType<BoatSliderScript>();
        boatEmitter = FindObjectOfType<BoatEmitter>();
        
        splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        splashEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/hook_splash");
        //splashEvent.start();
        
        lakeAmbienceEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/ambience_lake");
        lakeAmbienceEvent.start();
        
        
    }

    private void Update()
    {
        if (boatSliderScript.depthMathTotal < -0.5f)
        {
            hookSubmerged = true;
        }
        else hookSubmerged = false;
        
        
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
        if (other.gameObject.CompareTag("Bait") && boatSliderScript.depthMathTotal > -0.5f)
        {
            // For play lake ambience, maybe move me lateR?
            //hookSubmerged = !hookSubmerged; //swap bool mode BROKEN
            
            
             
            
            
            
            //Play FMOD Splash if not already playing to avoid duplicates
            FMOD.Studio.PLAYBACK_STATE pbState;
            splashEvent.getPlaybackState(out pbState);
            if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/hook_splash", other.gameObject.transform.position);
                // splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.gameObject));
                // FMODUnity.RuntimeManager.AttachInstanceToGameObject(splashEvent, other.gameObject.transform);
                
                // Using Attatch the sound will stay on target.
                // To3DAttributes is like playOneShot. It is at that position once. not updated
                
                splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.gameObject));
                splashEvent.start();
               
                // No need to release this from memory. It is 1 sound played once at times.
            }
        }
    }
}