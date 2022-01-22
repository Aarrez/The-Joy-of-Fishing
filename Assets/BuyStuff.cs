using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyStuff : MonoBehaviour
{
    public Button mindcontrol, lvl1Line, lvl2Line, lvl3Line;
    public uint mindControlCost, lvl1Cost, lvl2Cost, lvl3Cost;
    public int lvl1Length, lvl2Length, lvl3Lenght;
    MoneyEffect callMoneyEffectScript;
    BoatScript callBoatScript;

    private void Start() 
    {
    callMoneyEffectScript = FindObjectOfType<MoneyEffect>();   
    callBoatScript = FindObjectOfType<BoatScript>(); 
    }

    public void BuyMindControlLure()
    {
        if (callMoneyEffectScript.totalMoney >= mindControlCost)
        {
            mindcontrol.interactable = false;
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
            callBoatScript.maxLineLength = lvl1Length;
            callMoneyEffectScript.totalMoney = callMoneyEffectScript.totalMoney - lvl1Cost;
        }
        else
        {
            Debug.Log("not enough money!");
        }
    }

    public void BuyLineLVL2()
    {
        if (callMoneyEffectScript.totalMoney >= lvl2Cost)
        {
            lvl1Line.interactable = false;
            lvl2Line.interactable = false;
            callBoatScript.maxLineLength = lvl2Length;
            callMoneyEffectScript.totalMoney = callMoneyEffectScript.totalMoney - lvl1Cost;
        }
        else
        {
            Debug.Log("not enough money!");
        }
    }

    public void BuyLineLVL3()
    {
        if (callMoneyEffectScript.totalMoney >= lvl3Cost)
        {
            lvl1Line.interactable = false;
            lvl2Line.interactable = false;
            lvl3Line.interactable = false;
            callBoatScript.maxLineLength = lvl3Lenght;
            callMoneyEffectScript.totalMoney = callMoneyEffectScript.totalMoney - lvl1Cost;
        }
        else
        {
            Debug.Log("not enough money!");
        }
    }
    
}
