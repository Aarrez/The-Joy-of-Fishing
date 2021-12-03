using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CollectFish : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Transform> fishList = new List<Transform>();

    private Transform hook;

    private Transform fishInventory;

    private float distToCollectFish = 5f;

    private void Awake()
    {
        hook = this.transform.GetChild(0);
        fishInventory = GameObject.FindGameObjectWithTag("FishInventory").transform;
    }

    private void Update()
    {
        float dist = Vector3.Distance(this.transform.position, hook.position);
        if (dist < distToCollectFish)
        {
            for (int i = 0; i < hook.childCount; i++)
            {
                if (hook.GetChild(i).gameObject.activeInHierarchy)
                {
                    hook.GetChild(i).gameObject.SetActive(false);
                    hook.GetChild(i).parent = fishInventory;
                }
            }
        }
    }
}