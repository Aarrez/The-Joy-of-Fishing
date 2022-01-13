using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoppescript : MonoBehaviour
{
    public List<GameObject> ShopList;
    public List<Transform> ShopListPosition;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (GameObject ShopItem in ShopList)
        {
            if (i <= ShopListPosition.Count)
            {
                Object.Instantiate(ShopItem, ShopListPosition[i].position, ShopListPosition[i].rotation);
                
                i++;
            }
            else break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
