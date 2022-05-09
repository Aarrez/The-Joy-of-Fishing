using UnityEngine;

/*
 * The CollectFish script takes the children of the gameobject taged with "Bait"
 * and puts them in the gameobject with the tag "FishInventory".
 */

public class CollectFish : MonoBehaviour
{
    private Transform hook;

    private Transform fishInventory;

    private GameObject fishCollective;

    //[SerializeField] private float distToCollectFish = 5f;

    private bool CanCatchFish = false;

    //Used in CollectFish and MoneyEffect
    public static event System.Action DoneCollecting;

    private void OnEnable()
    {
        BaitScript.BaitIsOut += FindHookAndInventory;
        MoneyEffect.DeleteFish += ClearFishCollection;
        PlayerScript.DoneFishing += AddFishToInventory;
    }

    private void OnDisable()
    {
        BaitScript.BaitIsOut -= FindHookAndInventory;
        MoneyEffect.DeleteFish -= ClearFishCollection;
        PlayerScript.DoneFishing -= AddFishToInventory;
    }

    private void FindHookAndInventory(bool bait)
    {
        if (bait)
        {
            try { fishInventory = GameObject.FindGameObjectWithTag("FishInventory").transform; }
            catch
            {
                GameObject fishGameObject = new GameObject();
                fishGameObject.tag = "FishInventory";
                fishGameObject.name = "FishCollection";
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

    private void AddFishToInventory()
    {
        fishCollective = GameObject.FindGameObjectWithTag("FishCollective");
        for (int i = 0; i < fishCollective.transform.childCount; i++)
        {
            if (fishCollective.transform.GetChild(i).CompareTag("Fish"))
            {
                fishCollective.transform.GetChild(i).gameObject.SetActive(false);
                fishCollective.transform.GetChild(i).parent = fishInventory;
                BaitScript.FishOfHook?.Invoke();
            }
        }
        DoneCollecting?.Invoke();
    }

    private void ClearFishCollection()
    {
        for (int i = 0; i < fishInventory.childCount; i++)
        {
            Destroy(fishInventory.GetChild(i).gameObject);
        }
    }
}