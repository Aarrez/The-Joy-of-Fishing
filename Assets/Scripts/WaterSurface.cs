using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    private FMOD.Studio.EventInstance splashEvent;

    private void Awake()
    {
        splashEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/hook_splash");
        //splashEvent.start();
    }

    private void OnTriggerEnter2D(Collider2D other) //WaterSurface need to be BaitLayer. This ok?
    {
        if (other.gameObject.CompareTag("Bait"))
        {
            FMOD.Studio.PLAYBACK_STATE pbState;
            splashEvent.getPlaybackState(out pbState);
            if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/hook_splash", other.gameObject.transform.position);
                // splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.gameObject));
                // FMODUnity.RuntimeManager.AttachInstanceToGameObject(splashEvent, other.gameObject.transform);
                
                // Using Attatch the sound will stay on target.
                // To3DAttributes is like playOneShot. It is at that position once. not updated
                
                splashEvent.start();
                splashEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(other.gameObject));
               
                // No need to release this from memory. It is 1 sound played once at times.
            }
        }
    }
}