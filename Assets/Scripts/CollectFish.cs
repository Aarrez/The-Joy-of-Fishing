using UnityEngine;

/*
 * The CollectFish script takes the children of the gameojbect taged with "Bait"
 * and puts them in the gameobject with the tag "FishInventory".
 */

public class CollectFish : MonoBehaviour
{
    private Transform hook;

    private Transform fishInventory;

    private float distToCollectFish = 5f;

    private bool CanCatchFish = false;

    private void OnEnable()
    {
        BaitScript.BaitIsOut += FindHookAndInventory;
    }

    private void OnDisable()
    {
        BaitScript.BaitIsOut -= FindHookAndInventory;
    }

    //Need to add to Main Scene: A object with fishinventory tag
    private void FindHookAndInventory()
    {
        try { fishInventory = GameObject.FindGameObjectWithTag("FishInventory").transform; }
        catch { fishInventory = new GameObject().transform; }

        hook = FindObjectOfType<BaitScript>().transform;
        CanCatchFish = true;
    }

    private void Update()
    {
        if (!CanCatchFish) { return; }

        float dist = Vector3.Distance(this.transform.position, hook.position);
        if (dist < distToCollectFish)
        {
            for (int i = 0; i < hook.childCount; i++)
            {
                if (hook.GetChild(i).gameObject.activeInHierarchy)
                {
                    hook.GetChild(i).gameObject.SetActive(false);
                    hook.GetChild(i).parent = fishInventory;
                }
            }
        }
    }
}