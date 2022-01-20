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

    private GameObject CollectiveFish;

    //Used in BaitScrip and MoveAi
    public static Func<int> BaitLevel;

    //Used in BaitScript, CollectFish, MoneyEffect and MoveAi
    public static event Action<bool> BaitIsOut;

    //Used in BaitScript and MoneyEffect
    public static Action FishOnHook;

    //Used in BaitScript and CollectFish
    public static Action FishOfHook;

    private void OnEnable()
    {
        AddFishCollective();

        BaitLevel += delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(true);

        FishOnHook += ChangeBaitLevel;
        FishOfHook += ChangeBaitLevel;
        FishOnHook += PlaySound;
    }

    private void OnDisable()
    {
        BaitLevel -= delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(false);

        FishOnHook -= ChangeBaitLevel;
        FishOfHook -= ChangeBaitLevel;
        FishOnHook -= PlaySound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        #region Return if:s

        if (!collision.collider.CompareTag("Fish")) { return; }

        if (collision.transform.GetComponent<FishStats>().fishStats.baitLevel != currentBait) { return; }

        #endregion Return if:s

        collision.transform.parent = CollectiveFish.transform;
        
        if (CollectiveFish.transform.childCount == 2)
        {
            FishEatFish();
        }

        AddFishToHook();

        
    }

    private void AddFishCollective()
    {
        CollectiveFish = new GameObject();
        CollectiveFish.transform.parent = this.transform;
        CollectiveFish.tag = "FishCollective";
        CollectiveFish.name = "Collective";
        CollectiveFish.transform.position = Vector3.zero;
    }

    private void AddFishToHook()
    {
        for (int i = 0; i < CollectiveFish.transform.childCount; i++)
        {
            if (CollectiveFish.transform.GetChild(i).CompareTag("Fish"))
            {
                Transform colChild = CollectiveFish.transform.GetChild(i);
                colChild.GetComponent<Collider2D>().enabled = false;
                colChild.GetComponent<MoveAi>().enabled = false;
                colChild.GetComponent<Pathfinding.AIPath>().enabled = false;
                colChild.GetComponent<Rigidbody2D>().isKinematic = true;
                colChild.GetComponentInChildren<Animator>().SetBool("Moveing", false);
                colChild.position = this.transform.position;
                colChild.rotation = Quaternion.Euler(0, 0, 0);
                FishOnHook?.Invoke();
            }
        }
    }

    private void FishEatFish()
    {
        int LowestBaitLevel = 0;
        for (int i = 0; i < CollectiveFish.transform.childCount; i++)
        {
            if (CollectiveFish.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel < LowestBaitLevel)
            {
                LowestBaitLevel = CollectiveFish.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel;
            }
        }
        Destroy(CollectiveFish.transform.GetChild(LowestBaitLevel).gameObject);
        GetComponentInChildren<ParticleSystem>().Play();
    }

    private void ChangeBaitLevel()
    {
        int highesBaitLevel = 0;
        for (int i = 0; i < CollectiveFish.transform.childCount; i++)
        {
            if (CollectiveFish.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel > highesBaitLevel)
            {
                highesBaitLevel = CollectiveFish.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel;
            }
        }
        
        currentBait = CollectiveFish.transform.GetChild(highesBaitLevel).GetComponent<FishStats>().fishStats.baitLevel + 1;
        currentBait = Mathf.Clamp(currentBait, 0, 4);

        if (CollectiveFish.transform.childCount == 0)
        {
            currentBait = 0;
        }

        Debug.Log("BaitLevel: " + currentBait);
    }

    private void PlaySound()
    {
        fishHookedInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/fish_hook");
        fishHookedInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        fishHookedInstance.start();
        fishHookedInstance.release();
    }
}