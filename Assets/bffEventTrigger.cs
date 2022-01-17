using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bffEventTrigger : MonoBehaviour
{

    public Animator bffanim;
    private FMOD.Studio.EventInstance inst;

    void Awake()
    {
        inst = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/bff_trigger");
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Bait")
        {
            bffanim.Play("Swimmer");
            inst.start();
            inst.release();
        }
    }

    public void endgame()
    {
        GameManager.instance.UIScreenfadeout();
    }
}
