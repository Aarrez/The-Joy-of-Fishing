using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    private void OnEnable()
    {
        BoatScript.DoneFishing += GetMoney;
    }

    private void OnDisable()
    {
        BoatScript.DoneFishing -= GetMoney;
    }

    private void GetMoney()
    {
        Debug.Log("Gets money");
    }
}
