using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class RadioMusic : MonoBehaviour
{
    /*private FMOD.Studio.EventInstance radioInstance;
    private MusicPlayer musicPlayer;

    private void Awake()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        radioInstance = FMODUnity.RuntimeManager.CreateInstance("event:/radio_music");
        radioInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }
    
    public void PlayRadio()
    {
        radioInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        
        musicPlayer.DuckMusic(); //lower music volume

        FMOD.Studio.PLAYBACK_STATE pbState;
        radioInstance.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            radioInstance.start();
        }
    }

    public void StopRadio()
    {
        musicPlayer.RaiseMusic();
        radioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //radioInstance.release();
    }*/

}
