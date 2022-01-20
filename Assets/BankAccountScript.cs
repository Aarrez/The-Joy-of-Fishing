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
    float elapsed;
    void Start()
    {
        callMoneyEffectScript = FindObjectOfType<MoneyEffect>();
        BankText = GetComponent<TextMeshProUGUI>();
        if (thisIs2)
        {
            thisIs2rb = GetComponent<Rigidbody2D>();
            thisIs2transform = GetComponent<Transform>();
        }

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

        if (GameManager.instance.moveCam != 3 && thisIs2 && callMoneyEffectScript.totalMoney > 0)
        {
            elapsed += Time.deltaTime;
            BankText.text = "Golly, you got fish! " + callMoneyEffectScript.fractionMoney + " COINERS";
            thisIs2rb.velocity = new Vector2(0, 1);
            if(elapsed >= 3 && elapsed <= 6)
            {
                BankText.color = new Color(1, 1, 1, 0);
                thisIs2rb.velocity = new Vector2(0, -1);
            }
            if(elapsed > 6)
            {
                thisIs2rb.velocity = Vector2.zero;
            }

            //Debug.Log("elapsed" + elapsed);
        }else if (GameManager.instance.moveCam == 3 && thisIs2)
        {
            elapsed = 0f;
            Debug.Log("elapsed reset to: " + elapsed);
        }
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
