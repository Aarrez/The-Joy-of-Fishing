using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMoveSound : MonoBehaviour
{
    private FMOD.Studio.EventInstance fishMoveEvent;
    [SerializeField] private FMODUnity.EventReference fmodEvent; //allows us to choose event in Inspector


    [SerializeField] [Range(1, 3)] private int size; //1 to 3, small, medium, big.

    void Awake()
    {
        fishMoveEvent = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        fishMoveEvent.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(fishMoveEvent, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        fishMoveEvent.setParameterByName("Size", size);

        //loop for testing 
        FMOD.Studio.PLAYBACK_STATE pbState;
        fishMoveEvent.getPlaybackState(out pbState);
        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            fishMoveEvent.start();
        }
    }
}