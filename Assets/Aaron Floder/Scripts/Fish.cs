using System;
using UnityEngine;

/*
 * Creates a asset with stats to more easely make fish.
 */

[CreateAssetMenu(fileName = "FishStats", order = 0)]
public class Fish : ScriptableObject
{
    [SerializeField] private string fishName = "";

    [Tooltip("In g")]
    public float weight;

    [Tooltip("In cm")]
    public float length;

    [Tooltip("The amount of money you get when the fish is sold")]
    [SerializeField] public float value = 10f;

    [SerializeField] private float baitAttractionRadius = 10f;

    public Color fishColor;

    private enum Bait : int
    {
        Worm = 0,
        SmallerFish = 1,
        Stone = 2
    }

    [Range(0, 4)] [SerializeField] private uint struggleCount = 0;

    [Range(0, 1)] [Unity.Collections.ReadOnly, SerializeField] protected int excitementLevel;

    private void OnEnable()
    {
        if (weight > 100 && length > 10)
        {
            excitementLevel = 1;
        }
        else
        {
            excitementLevel = 0;
        }
    }
}