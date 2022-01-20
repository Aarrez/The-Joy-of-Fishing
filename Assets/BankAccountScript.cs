using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BankAccountScript : MonoBehaviour
{
    MoneyEffect callMoneyEffectScript;
    TextMeshProUGUI BankText;
    public bool thisIs1;
    public bool thisIs2;
    void Start()
    {
        callMoneyEffectScript = FindObjectOfType<MoneyEffect>();
        BankText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.moveCam != 3)
        {
            BankText.text = "Dolluh$: " + callMoneyEffectScript.totalMoney.ToString() + "c";
        }
        //else if (GameManager.instance.moveCam == 3)
        //{
        //    BankText.text = "";
        //}

        //if (GameManager.instance.moveCam != 3 && thisIs2)
        //{
        //    BankText.text = "MoneyBank: " + callMoneyEffectScript.coinsBanked.ToString();
        //}
        ////else if (GameManager.instance.moveCam == 3)
        ////{
        ////    BankText.text = "";
        ////}
    }
}
