using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEmitter : MonoBehaviour
{
    private FMOD.Studio.EventInstance inst;

    private FMOD.Studio.EventInstance instUnderwater;
    //

    private void Awake()
    {
        instUnderwater.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instUnderwater = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/ambience_boat_underwater");

        // Make sure its muted at start
        instUnderwater.setParameterByName("music_duck", 1); // Mute

        inst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        inst = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/ambience_boat");

        inst.start();
        instUnderwater.start();
    }

    private void Update()
    {
        instUnderwater.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        inst.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    public void UnderWater()
    {
        instUnderwater.setParameterByName("music_duck", 0); // Play
        inst.setParameterByName("music_duck", 1); // Mute
    }

    public void AboveWater()
    {
        instUnderwater.setParameterByName("music_duck", 1); // Mute
        inst.setParameterByName("music_duck", 0); // Play
    }
}