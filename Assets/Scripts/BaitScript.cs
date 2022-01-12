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

    [Range(0, 2)] [SerializeField] int currentBait;

    public static Func<int> BaitLevel;

    public static event Action BaitIsOut;

    private void OnEnable()
    {
        BaitLevel += delegate () { return bait[currentBait].baitLevel; };
        BaitIsOut();
    }

    private void OnDisable()
    {
        BaitLevel -= delegate () { return bait[currentBait].baitLevel; };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Fish")
        {
            collision.transform.parent = this.transform;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}