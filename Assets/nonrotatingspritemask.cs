using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonrotatingspritemask : MonoBehaviour
{
    Quaternion fixedRotation;
    private void Awake()
    {
        Quaternion fixedRotation = transform.rotation;
    }
    private void LateUpdate()
    {
        transform.rotation = fixedRotation;
    }
}
