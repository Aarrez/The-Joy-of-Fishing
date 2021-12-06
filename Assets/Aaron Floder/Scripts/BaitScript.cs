using System;
using UnityEngine;

/* This script is for when you have cought a fish.
 * It makes the fish a child of the transform the script is on.
 */

public class BaitScript : MonoBehaviour
{
    public static event Action FishCought;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Fish")
        {
            FishCought?.Invoke();
            collision.transform.parent = this.transform;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}