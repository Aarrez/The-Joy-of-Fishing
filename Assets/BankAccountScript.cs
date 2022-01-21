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
    Rigidbody2D thisIs2rb;
    Transform thisIs2transform;
    public float elapsed;
    void Start()
    {
        callMoneyEffectScript = FindObjectOfType<MoneyEffect>();
        BankText = GetComponent<TextMeshProUGUI>();
        if (thisIs2)
        {
            thisIs2rb = GetComponent<Rigidbody2D>();
            thisIs2transform = GetComponent<Transform>();
        }
        BankText.text = "";

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.moveCam != 3 && thisIs1)
        {
            BankText.text = "Savings: " + callMoneyEffectScript.totalMoney.ToString() + "c";
        }
        //else if (GameManager.instance.moveCam == 3)
        //{
        //    BankText.text = "";
        //}

        if (thisIs2 && callMoneyEffectScript.totalMoney > 0 && callMoneyEffectScript.gainedNow == true)
        {
            
            elapsed += Time.deltaTime;
            BankText.text = "Gains: " + callMoneyEffectScript.fractionMoney + "c";
            thisIs2rb.velocity = new Vector2(0, 1);
            if(elapsed <= 0.2)
            {
                BankText.color = new Color(1, 1, 1, 1);
            }
            if (elapsed >= 3 && elapsed <= 4f)
            {
                BankText.color = new Color(1, 1, 1, 0);
                thisIs2rb.velocity = new Vector2(0, -3);
            }
            if (elapsed > 4f)
            {
                thisIs2rb.velocity = Vector2.zero;
                callMoneyEffectScript.gainedNow = false;
            }
        } 
        //else if (thisIs2 && callMoneyEffectScript.gainedNow == false)
        //{
        //    elapsed += Time.deltaTime;
        //    if (elapsed >= 3 && elapsed <= 6)
        //    {
        //        BankText.color = new Color(1, 1, 1, 0);
        //        thisIs2rb.velocity = new Vector2(0, -1);
        //    }
        //    if (elapsed > 6)
        //    {
        //        thisIs2rb.velocity = Vector2.zero;
        //    }
        //    //thisIs2rb.velocity = Vector2.zero;
        //    //elapsed = 0f;
        //    //Debug.Log("elapsed reset to: " + elapsed);
        //}
        ////else if (GameManager.instance.moveCam == 3)
        ////{
        ////    BankText.text = "";
        ////}
    }

    IEnumerator MoneyGainedTextVector()
    {

        yield return new WaitForSeconds(3);
        thisIs2rb.velocity = Vector2.zero;
    }
}
