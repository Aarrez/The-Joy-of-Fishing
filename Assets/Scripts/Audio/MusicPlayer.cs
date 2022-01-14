using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicInstance;

    private void Awake()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
    }

    private void Start()
    {
        StartMusic();
    }

    private void StartMusic()
    {
        FMOD.Studio.PLAYBACK_STATE pbState;
        musicInstance.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            musicInstance.start();
        }
    }

    public void DuckMusic()
    {
        musicInstance.setParameterByName("music_duck", 1); //1 = yes
    }

    public void RaiseMusic()
    {
        musicInstance.setParameterByName("music_duck", 0); //dont duck music
    }
}
