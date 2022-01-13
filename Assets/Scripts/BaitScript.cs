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

    [Range(0, 1)] [SerializeField] int currentBait;

    public static Func<int> BaitLevel;

    //Used to get in ColletFish and in MoveAi to get when the bait is out.
    public static event Action<bool> BaitIsOut;

    private void OnEnable()
    {
        BaitLevel += delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(true);
    }

    private void OnDisable()
    {
        BaitLevel -= delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Fish"))
        {
            if (collision.transform.GetComponent<FishStats>().fishStats.baitLevel != currentBait) { return; }
            else if (this.transform.childCount > 0) { return; }
            
            collision.transform.parent = this.transform;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
                this.transform.GetChild(i).GetComponent<MoveAi>().enabled = false;
                this.transform.GetChild(i).GetComponent<Pathfinding.AIPath>().enabled = false;
                this.transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = true;

            }
        }                                                                      
    }
}