using UnityEngine;

/*
 * The CollectFish script takes the children of the gameojbect taged with "Bait"
 * and puts them in the gameobject with the tag "FishInventory".
 */

public class CollectFish : MonoBehaviour
{
    private Transform hook;

    private Transform fishInventory;

    [SerializeField] private float distToCollectFish = 5f;

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
    private void FindHookAndInventory(bool bait)
    {
        if (bait)
        {
            try { fishInventory = GameObject.FindGameObjectWithTag("FishInventory").transform; }
            catch 
            {
                GameObject fishGameObject = new GameObject();
                fishInventory = fishGameObject.transform;
            }

            hook = FindObjectOfType<BaitScript>().transform;
            CanCatchFish = bait;
        }
        else
        {
            CanCatchFish = bait;
        }
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