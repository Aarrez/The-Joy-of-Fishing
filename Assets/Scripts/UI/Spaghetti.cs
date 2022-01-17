using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spaghetti : MonoBehaviour
{
    // Start is called before the first frame update

    public Button mindcontrol, level1,level2,level3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMindControl()
    {
        GameManager.instance.currentBait = 1;
    }

    public void GetLevel1()
    {

    }

    public void GetLevel2()
    {

    }

    public void GetLevel3()
    {

    }
}
