using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicInstance;
    
    [Header("toggle me on to not start music at play")]
    [SerializeField] private bool DEBUG_DONT_PLAY;

    private void Awake()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
    }

    private void Start()
    {
        if (!DEBUG_DONT_PLAY)
        {
            StartMusic();
        }
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

    public void StopMusic() // This is played at the end of the game!
    {
        musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}