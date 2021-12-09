using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePartScript : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    Transform fishHook;
    void Start()
    {
        ResetAnchor();
        fishHook = GameObject.Find("playercontrolropetest").GetComponent<Transform>();

    }


    public void ResetAnchor()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopePartScript abovePart = connectedAbove.GetComponent<RopePartScript>();
        if (abovePart != null)
        {
            abovePart.connectedBelow = gameObject;
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
        }
        else 
        { 
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);  
        } //this segment is the top one
    }

}
