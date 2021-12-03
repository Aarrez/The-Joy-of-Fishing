using System;
using UnityEngine;
using FMODUnity;

public class BaitScript : MonoBehaviour
{
    public static event Action FishCought;

    public static event Func<int> FishOnHook;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Fish")
        {
            FishCought?.Invoke();
            collision.transform.parent = this.transform;
            FishOnHook = delegate () { return this.transform.childCount; };
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}