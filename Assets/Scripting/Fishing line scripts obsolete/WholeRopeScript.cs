using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeRopeScript : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] prefabRopeParts;
    public int numLinks = 5; //CHANGEABLE

    public HingeJoint2D top;


    void Start()
    {
        GenerateRope();
    }


    void GenerateRope()
    {
        Rigidbody2D prevBod = hook;
        for(int i = 0; i < numLinks; i++)
        {
            int index = Random.Range(0, prefabRopeParts.Length);
            GameObject newPart = Instantiate(prefabRopeParts[index]);
            newPart.transform.parent = transform;
            newPart.transform.position = transform.position;
            HingeJoint2D hj = newPart.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            prevBod = newPart.GetComponent<Rigidbody2D>();

            if (i == 0)
            {
                top = hj;
            }
        }
    }

    public void AddLink()
    {
        int index = Random.Range(0, prefabRopeParts.Length);
        GameObject newLink = Instantiate(prefabRopeParts[index]);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position;
        HingeJoint2D hj = newLink.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;
        newLink.GetComponent<RopePartScript>().connectedBelow = top.gameObject;
        top.connectedBody = newLink.GetComponent<Rigidbody2D>();
        top.GetComponent<RopePartScript>().ResetAnchor();
        top = hj;
    }

    public void RemoveLink()
    {
        HingeJoint2D newTop = top.gameObject.GetComponent<RopePartScript>().connectedBelow.GetComponent<HingeJoint2D>();
        newTop.connectedBody = hook;
        newTop.gameObject.transform.position = hook.gameObject.transform.position;
        newTop.GetComponent<RopePartScript>().ResetAnchor();
        Destroy(top.gameObject);
        top = newTop;
    }

}
