using UnityEngine;

public class FishStats : MonoBehaviour
{
    public Fish fishStats;

    public SpriteRenderer sprRend;


    private void Start()
    {
        sprRend = GetComponentInChildren<SpriteRenderer>();
        SetFishStats();
    }

    private void SetFishStats()
    {
        sprRend.sprite = fishStats.sprite;

        sprRend.color = fishStats.fishColor;

        GetComponentInChildren<Animator>().runtimeAnimatorController = fishStats.animatorController;
        GetComponentInChildren<Animator>().SetBool("Moveing", true);
    }
}