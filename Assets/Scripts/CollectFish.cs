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

    private void Awake()
    {
        try { hook = GameObject.FindGameObjectWithTag("Bait").transform; }
        catch
        {
            Debug.Log($"There is no Gameobject with the tag 'Bait'");
        }
        fishInventory = GameObject.FindGameObjectWithTag("FishInventory").transform;
    }

    private void Update()
    {
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