using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSound : MonoBehaviour
{
    private FMOD.Studio.EventInstance inst;
    private bool triggered;
    private MusicPlayer musicPlayer;

    void Awake()
    {
        inst = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/bff_trigger");
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Bait" && !triggered)
        {
            musicPlayer.DuckMusic();
            inst.start();
            inst.release();
            triggered = true;
            Invoke("DoMusic", 29f);
        }
    }

    void DoMusic()
    {
        musicPlayer.RaiseMusic();
    }
}
