using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBobbingScript : MonoBehaviour
{
    float iniXvalue;
    public float xBobbingStrength = 1;
    public bool applyBobOffset;
    public float bobOffsetAmount;
    void Start()
    {
        this.iniXvalue = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(applyBobOffset == true)
        {
            transform.position = new Vector3(iniXvalue + ((float)Mathf.Sin(Time.time + bobOffsetAmount) * xBobbingStrength), transform.position.y, transform.position.z);
        }

        if (applyBobOffset == false)
        {
            transform.position = new Vector3(iniXvalue + ((float)Mathf.Sin(Time.time) * xBobbingStrength), transform.position.y, transform.position.z);
        }
    }
}
