using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreation : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabRope;

    [SerializeField] private GameObject bait;

    private HingeJoint2D top;

    private HingeJoint2D bottom;

    private int baitIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < prefabRope.Length - 1; i++)
        {
            if (prefabRope[i].CompareTag("Bait"))
            {
                baitIndex = i;
            }
        }
    }

    public void AddLine()
    {
        GameObject newJoint = prefabRope[Random.Range(0, prefabRope.Length - 1)];
        newJoint = GameObject.Instantiate<GameObject>(newJoint);

        newJoint.transform.parent = transform;
    }
}