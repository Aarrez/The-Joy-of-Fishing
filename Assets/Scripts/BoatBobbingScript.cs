using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBobbingScript : MonoBehaviour
{

    float iniYvalue;
    public float bobbingStrength = 1;
    public bool applyBobOffset;
    public float bobOffsetAmount;


    void Start()
    {
            this.iniYvalue = this.transform.position.y;
    }

    void Update()
    {

        if(applyBobOffset == true)
        {
            transform.position = new Vector3(transform.position.x, iniYvalue + ((float)Mathf.Sin(Time.time + bobOffsetAmount) * bobbingStrength), transform.position.z);
        }

        if(applyBobOffset == false)
        {
            transform.position = new Vector3(transform.position.x, iniYvalue + ((float)Mathf.Sin(Time.time) * bobbingStrength), transform.position.z);
        }

    }

}
