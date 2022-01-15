using System;
using UnityEngine;

/*
 * This script is for when you have cought a fish.
 * It makes the fish a child of the transform the script is on.
 */

[RequireComponent(typeof(Collider2D))]
public class BaitScript : MonoBehaviour
{
    [SerializeField] private BaitScriptAbleObject[] bait;

    [SerializeField] private int currentBait;

    public static Func<int> BaitLevel;

    //Used to get in ColletFish and in MoveAi to get when the bait is out.
    public static event Action<bool> BaitIsOut;

    public static Action FishOnHook;
    public static Action FishOfHook;

    private void OnEnable()
    {
        BaitLevel += delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(true);

        FishOnHook += ChangeBaitLevel;
        FishOfHook += ChangeBaitLevel;
    }

    private void OnDisable()
    {
        BaitLevel -= delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(false);

        FishOnHook -= ChangeBaitLevel;
        FishOfHook -= ChangeBaitLevel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Fish"))
        {
            if (collision.transform.GetComponent<FishStats>().fishStats.baitLevel > currentBait) { return; }
            else if (this.transform.childCount > 1) { return; }

            collision.transform.parent = this.transform;
            if (this.transform.childCount == 2)
            {
                Destroy(this.transform.GetChild(0).gameObject);
            }
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
                this.transform.GetChild(i).GetComponent<MoveAi>().enabled = false;
                this.transform.GetChild(i).GetComponent<Pathfinding.AIPath>().enabled = false;
                this.transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = true;
                FishOnHook?.Invoke();
            }
        }
    }

    private void ChangeBaitLevel()
    {
        if (this.transform.childCount > 0)
        {
            currentBait = this.transform.GetChild(0).GetComponent<FishStats>().fishStats.baitLevel + 1;
        }
        else
        {
            currentBait = 0;
        }
    }
}