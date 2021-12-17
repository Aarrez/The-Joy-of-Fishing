using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance volumeTestEvent;

    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus sfx;
    private FMOD.Studio.Bus master; //needed?
    private FMOD.Studio.Bus ambience;
    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;
    private float masterVolume = 1f;
    private float ambienceVolume = 0.05f;
    // Any sliders connected to these values must on start have the same in insepctor component.
    // Slider component's "Value" setting Must match these float values.
    
    void Awake()
    {
        master = FMODUnity.RuntimeManager.GetBus("bus:/");
        ambience = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        volumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/VolumeTest");
    }

    // Update is called once per frame
    void Update()
    {
        // FMOD bus volume 1 = whatever its set in FMOD Studio. 0-1 represents the distance between that and inf.
        music.setVolume(musicVolume);
        sfx.setVolume(sfxVolume);
        ambience.setVolume(ambienceVolume);
        master.setVolume(masterVolume);
        //does this chug framerate? maybe not have it in update? idk how FMOD likes to do things.
    }


    public void MasterVolumeLevel(float newMasterVolume)
    {
        masterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }

    public void SfxVolumeLevel(float newSfxVolume)
    {
        sfxVolume = newSfxVolume;
        
        //for example volume sfx play
        FMOD.Studio.PLAYBACK_STATE pbState;
        volumeTestEvent.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            volumeTestEvent.start();
        }
    }

    public void AmbienceVolumeLevel(float newAmbienceVolume)
    {
        ambienceVolume = newAmbienceVolume;
    }
    

}