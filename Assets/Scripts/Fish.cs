using UnityEditor.Animations;
using UnityEngine;

/*
 * Creates a asset with stats to easily make fish.
 */

[CreateAssetMenu(fileName = "FishStats", order = 0)]
public class Fish : ScriptableObject
{
    public Sprite sprite;

    public AnimatorController animatorController;

    public string fishName = "";

    [Header("The length and weight affects " +
        "excitementLevel, struggleCount, baitLevel")]
    [Tooltip("In cg")]
    public float weight;

    [Tooltip("In dm")]
    public float length;

    [Tooltip("The amount of money you get when the fish is sold")]
    public float value = 10f;

    public float baitAttractionRadius = 10f;

    public Color fishColor = Color.white;

    public int struggleCount;

    public int excitementLevel;

    public int baitLevel;

    private void OnValidate()
    {
        CheckValue();
    }

    private void CheckValue()
    {
        float wlValue = weight + length;
        switch (wlValue)
        {
            case < 30f:

                excitementLevel = 1;
                struggleCount = Random.Range(0, 1);
                baitLevel = 0;
                break;

            case > 30f when wlValue < 60f:

                excitementLevel = 1;
                struggleCount = Random.Range(2, 3);
                baitLevel = 1;
                break;

            case > 60f when wlValue < 90f:

                excitementLevel = 2;
                struggleCount = 3;
                baitLevel = 2;
                break;

            default:
                excitementLevel = 2;
                struggleCount = 4;
                baitLevel = 3;
                break;
        }
    }
}