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
    }

    private void FindFishCollector(bool bait)
    {
        if (!hookedFish) { return; }

        FishCollector = GameObject.FindGameObjectWithTag("FishInventory");
    }

    private void GetMoney()
    {
        if (hookedFish) { return; }

        int a = 0;
        for (int i = 0; i < FishCollector.transform.childCount - 1; i++)
        {
            if (FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel > a)
            {
                a = FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel;
            }
        }

        coinParticle[a].Play();
    }



    
}