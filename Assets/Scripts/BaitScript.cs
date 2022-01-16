using System;
using UnityEngine;

/*
 * This script is for when you have cought a fish.
 * It makes the fish a child of the transform the script is on.
 */

[RequireComponent(typeof(Collider2D))]
public class BaitScript : MonoBehaviour
{
    private FMOD.Studio.EventInstance fishHookedInstance;

    [SerializeField] private BaitScriptAbleObject[] bait;

    [SerializeField] private int currentBait;

    [SerializeField] private GameObject BloodParticules;

    public static Func<int> BaitLevel;

    //Used to get in ColletFish and in MoveAi to get when the bait is out.
    public static event Action<bool> BaitIsOut;

    public static Action FishOnHook;
    public static Action FishOfHook;

    private void OnEnable()
    {
        BaitLevel += delegate() { return bait[currentBait].baitLevel; };
        BaitIsOut(true);

        FishOnHook += ChangeBaitLevel;
        FishOfHook += ChangeBaitLevel;
        FishOnHook += PlaySound;
    }

    private void OnDisable()
    {
        BaitLevel -= delegate() { return bait[currentBait].baitLevel; };
        BaitIsOut(false);

        FishOnHook -= ChangeBaitLevel;
        FishOfHook -= ChangeBaitLevel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Fish"))
        {
            if (collision.transform.GetComponent<FishStats>().fishStats.baitLevel != currentBait)
            {
                return;
            }
            else if (this.transform.childCount > 2)
            {
                return;
            }

            collision.transform.parent = this.transform;
            if (this.transform.childCount == 3)
            {
                Destroy(this.transform.GetChild(1).gameObject);
                GetComponentInChildren<ParticleSystem>().Play();
            }

            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).CompareTag("Fish"))
                {
                    this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
                    this.transform.GetChild(i).GetComponent<MoveAi>().enabled = false;
                    this.transform.GetChild(i).GetComponent<Pathfinding.AIPath>().enabled = false;
                    this.transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = true;
                    FishOnHook?.Invoke();
                }
            }
        }
    }

    private void ChangeBaitLevel()
    {
        if (this.transform.childCount > 1)
        {
            currentBait = this.transform.GetChild(1).GetComponent<FishStats>().fishStats.baitLevel + 1;
        }
        else
        {
            currentBait = 0;
        }
    }

    private void PlaySound()
    {
        fishHookedInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/fish_hook");
        fishHookedInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        fishHookedInstance.start();
        fishHookedInstance.release();
    }
}