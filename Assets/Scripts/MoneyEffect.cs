using System;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    private FMOD.Studio.EventInstance fishGetInstance;
    private FMOD.Studio.EventInstance coinsInstance;
    
    
    [SerializeField] private ParticleSystem[] coinParticle;

    private GameObject FishCollector;

    private bool hookedFish = false;

    public static event System.Action DeleteFish;

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
        if (!hookedFish || FishCollector.transform.childCount == 0) { return; }
        
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
        PlaySound(a);

        DeleteFish?.Invoke();
    }

    private void PlaySound(int level)
    {
        // Levels 0 1 2 3 small to big.
        coinsInstance.setParameterByName("level", level);
        coinsInstance.setParameterByName("triggered", 1);
        fishGetInstance.setParameterByName("level", level);
        
        coinsInstance.start();
        fishGetInstance.start();
        coinsInstance.setParameterByName("triggered", 0);
    }



    
}