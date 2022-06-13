using UnityEngine;
using Stem;

public class MoneyEffect : MonoBehaviour
{

    [Stem.SoundID]
    public Stem.ID UICoins = Stem.ID.None;

    [Stem.SoundID]

    public Stem.ID UICommonGet = Stem.ID.None;

    [Stem.SoundID]

    public Stem.ID UIRareGet = Stem.ID.None;

    [Stem.SoundID]

    public Stem.ID UILegendaryGet = Stem.ID.None;

    [Stem.SoundID]

    [SerializeField] private ParticleSystem[] coinParticle;

    private GameObject FishCollector;

    public uint totalMoney = 0;
    public uint fractionMoney = 0;
    public bool gainedNow;

    private bool hookedFish = false;

    public static event System.Action DeleteFish;
    public static event System.Func<uint> TheMoney;

    BankAccountScript callBankAccountScript;

    private void Awake()
    {
        //fishGetInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/fish_get");
        //coinsInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/coins");
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(fishGetInstance, gameObject.transform);
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(coinsInstnace, gameObject.transform);
        //coinsInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //fishGetInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        callBankAccountScript = FindObjectOfType<BankAccountScript>();
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
            fractionMoney = value;
            if (callBankAccountScript.thisIs2)
            {
                callBankAccountScript.elapsed = 0;
            }
            if (gainedNow == false)
            {
                gainedNow = true;
            }

        }
        //Debug.Log(totalMoney);


        coinParticle[a].Play();
        PlaySound(a);

        DeleteFish?.Invoke();
        TheMoney?.Invoke();
    }

    private void PlaySound(int level)
    {
        
        //coinsInstance.start();
        fishGetLevel(level);
    }

    private void fishGetLevel(int level)
    {
        if (level == 0) 
        {
            Stem.SoundManager.Play3D(UICommonGet, transform.position);
        }
        else if (level == 1)
        {
            Stem.SoundManager.Play3D(UIRareGet, transform.position);
        }
        else if (level == 2)
        {
            Stem.SoundManager.Play3D(UILegendaryGet, transform.position);
        }

    }


}