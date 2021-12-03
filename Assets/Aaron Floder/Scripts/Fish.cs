using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishStats", order = 0)]
public class Fish : ScriptableObject
{
    [SerializeField] private string fishName = "";

    [Tooltip("In g")]
    [SerializeField] private float weight;

    [Tooltip("In cm")]
    [SerializeField] private float length;

    [Tooltip("The amount of money you get when the fish is sold")]
    [SerializeField] private float value = 10f;

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