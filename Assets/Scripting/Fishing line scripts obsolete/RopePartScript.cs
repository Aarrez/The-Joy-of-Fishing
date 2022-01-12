using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePartScript : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    Transform fishHook;
    public float speed;
    private Vector3 currentDirection = Vector3.zero;
    Rigidbody2D rb;
    void Start()
    {
        ResetAnchor();
        fishHook = GameObject.Find("playercontrolropetest").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            //currentDirection = Vector3.zero;
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = 0;
            // rb.Sleep();
            //rb.isKinematic = true;
            //rb.simulated = false;
            
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Wall")
    //    {
    //        //currentDirection = Vector3.zero;
    //        //rb.velocity = Vector3.zero;
    //        //rb.angularVelocity = 0;
    //        // rb.Sleep();
    //        rb.isKinematic = false;
    //        rb.simulated = true;

    //        Debug.Log("EXITed");
    //    }
    //}


    void Update()
    {
        //Physics2D.IgnoreCollision(connectedAbove.GetComponent<BoxCollider2D>(), this.GetComponent<BoxCollider2D>());
        //if (currentDirection.Equals(Vector3.zero))
        //{
        //    Debug.Log("MEMES");
        //    Vector3 inputDirection = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        //    if (!inputDirection.Equals(Vector3.zero))
        //    {
        //        currentDirection = inputDirection;
        //        rb.velocity = currentDirection * speed;

        //    }

        //}
    }
}
