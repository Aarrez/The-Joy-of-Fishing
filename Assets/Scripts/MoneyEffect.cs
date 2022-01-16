using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] coinParticle;

    private GameObject FishCollector;

    private bool hookedFish = false;

    private void OnEnable()
    {
        BoatScript.DoneFishing += GetMoney;
        BaitScript.BaitIsOut += FindFishCollector;
        BaitScript.FishOnHook += IsFishOnHook;
    }

    private void OnDisable()
    {
        BoatScript.DoneFishing -= GetMoney;
        BaitScript.BaitIsOut -= FindFishCollector;
        BaitScript.FishOnHook -= IsFishOnHook;
    }

    private void IsFishOnHook()
    {
        hookedFish = true;
        Debug.Log("Can get money");
    }

    private void FindFishCollector(bool bait)
    {
        if (!hookedFish) { return; }

        FishCollector = GameObject.FindGameObjectWithTag("FishInventory");
    }

    private void GetMoney()
    {
        if (!hookedFish) { return; }
        Debug.Log("Gets Coins");
        int a = 0;
        for (int i = 0; i < FishCollector.transform.childCount; i++)
        {
            if (FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel > a)
            {
                a = FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel;
            }
        }
        Debug.Log(a);

        coinParticle[a].Play();
    }



    
}