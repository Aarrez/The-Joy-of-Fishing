using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    private FMOD.Studio.EventInstance fishGetInstance;
    private FMOD.Studio.EventInstance coinsInstance;

    [SerializeField] private ParticleSystem[] coinParticle;

    private GameObject FishCollector;

    private uint totalMoney = 0;

    private bool hookedFish = false;

    public static event System.Action DeleteFish;
    public static event System.Func<uint> TheMoney;

    private void Awake()
    {
        fishGetInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/fish_get");
        coinsInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/coins");
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(fishGetInstance, gameObject.transform);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(coinsInstnace, gameObject.transform);
        //coinsInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //fishGetInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    private void OnEnable()
    {
        CollectFish.DoneCollecting += EarnMoney;
        BaitScript.BaitIsOut += FindFishCollector;
        BaitScript.FishOnHook += IsFishOnHook;
        TheMoney = delegate () { return totalMoney; };
    }

    private void OnDisable()
    {
        CollectFish.DoneCollecting -= EarnMoney;
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

    private void EarnMoney()
    {
        if (FishCollector.transform.childCount == 0) { return; }

        int a = 0;
        uint[] b = new uint[FishCollector.transform.childCount];

        for (int i = 0; i < FishCollector.transform.childCount; i++)
        {
            b[i] = FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.value;
            if (FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel > a)
            {
                a = FishCollector.transform.GetChild(i).GetComponent<FishStats>().fishStats.baitLevel;
            }
        }

        foreach(uint value in b)
        {
            totalMoney += value;
        }
        Debug.Log(totalMoney);

        coinParticle[a].Play();
        PlaySound(a);

        DeleteFish?.Invoke();
        TheMoney?.Invoke();
    }

    private void PlaySound(int level)
    {
        // Levels 0 1 2 3 small to big.
        coinsInstance.setParameterByName("level", level);
        fishGetInstance.setParameterByName("level", level);

        coinsInstance.start();
        fishGetInstance.start();
    }
}