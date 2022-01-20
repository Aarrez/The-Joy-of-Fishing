using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BankAccountScript : MonoBehaviour
{
    MoneyEffect callMoneyEffectScript;
    TextMeshProUGUI BankText;
    void Start()
    {
        callMoneyEffectScript = FindObjectOfType<MoneyEffect>();
        BankText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.moveCam != 3)
        {
            BankText.text = "MoneyGane: " + callMoneyEffectScript.coinsGained.ToString();
        }else if(GameManager.instance.moveCam == 3)
        {
            BankText.text = "";
        }
    }
}
