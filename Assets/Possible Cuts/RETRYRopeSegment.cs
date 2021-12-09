using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RETRYRopeSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RETRYRopeSegment aboveSegment = connectedAbove.GetComponent<RETRYRopeSegment>();
        if (aboveSegment != null){
            aboveSegment.connectedBelow = gameObject;
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
        }
        else{
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        }


    }

}
