using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyStuff : MonoBehaviour
{
    public Button mindcontrol, lvl1Line, lvl2Line, lvl3Line;
    public uint mindControlCost, lvl1Cost, lvl2Cost, lvl3Cost;
    MoneyEffect callMoneyEffectScript;

    private void Start() 
    {
    callMoneyEffectScript = FindObjectOfType<MoneyEffect>();    
    }

    public void BuyMindControlLure()
    {
        if (callMoneyEffectScript.totalMoney >= mindControlCost)
        {
            mindcontrol.interactable = false;
            GameManager.instance.currentLineLevel = 0;
            callMoneyEffectScript.totalMoney = callMoneyEffectScript.totalMoney - mindControlCost;
        }
        else
        {
            Debug.Log("not enough money!");
        }
    }


    public void BuyLineLVL1()
    {
        if (callMoneyEffectScript.totalMoney >= lvl1Cost)
        {
            lvl1Line.interactable = false;
            GameManager.instance.currentLineLevel = 1;
            callMoneyEffectScript.totalMoney = callMoneyEffectScript.totalMoney - lvl1Cost;
        }
        else
        {
            Debug.Log("not enough money!");
        }
    }

    public void BuyLineLVL2()
    {
        
    }

    public void BuyLineLVL3()
    {
        
    }
    
}
