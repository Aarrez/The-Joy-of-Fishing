using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bffEventTrigger : MonoBehaviour
{

    public Animator bffanim;
    private FMOD.Studio.EventInstance inst;
    private MusicPlayer musicPlayer;
    private bool triggered;

    void Awake()
    {
        inst = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/bff_end2");
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Bait" && !triggered)
        {
            bffanim.Play("Swimmer");
            musicPlayer.StopMusic();
            inst.start();
            inst.release();
            //Invoke("endgame", 5.5f);
            triggered = true;
        }
    }

    public void endgame()
    {
        GameManager.instance.UIScreenfadeout();
    }
}
