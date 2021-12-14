﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public sealed class RopeScript : MonoBehaviour
{

    //holds where the hook is going to
    [HideInInspector]
    public Vector2 destiny;
    public static RopeScript instance;

    //velocity that the hook goes onto the destiny
    public float speed = 1;
    //distance between each node
    public float distance = 0.5f;
    //node prefab
    public GameObject nodePrefab;
    //hook prefab
    public GameObject hookPrefab;
    //rodtransform gameobject
    public GameObject rodtransform;
    //last node instantiated
    public GameObject lastNode;

    //line that represents rope
    public LineRenderer lr;

    //initial points on the rope (beginning and end)
    //public int vertexCount = 2;

    //list of all nodes instantiated
    public List<GameObject> Nodes = new List<GameObject>();

    //check if the full rope is created
    public bool done = false;

    //is something if an object with a rigidbody is hit
    public Transform target;

    //added hinge joint if there is relative object
    public HingeJoint2D hinge;

    //private Transform transform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one RopeScript");
        }
        print(gameObject.name);
    }
    // Use this for initialization
    void Start()
    {
        //sets the line renderer
        lr = GetComponent<LineRenderer>();
        //sets rodtransform
        if (rodtransform == null)
            rodtransform = GameObject.FindGameObjectWithTag("PlayerRod");


        lastNode = base.transform.gameObject;
        //Nodes.Add(transform.gameObject);


        //if hit an object
        Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //check if object has rigidbody
        if (col != null && col.GetComponent<Rigidbody2D>() != null)
        {

            //set it as the targe
            target = col.transform;

            //set hinge to dynamic
            base.transform.GetComponent<Rigidbody2D>().isKinematic = false;

            //get last hinge in inspector
            hinge = GetComponents<HingeJoint2D>()[1];

            //connect target's rigidbody
            hinge.connectedBody = col.GetComponent<Rigidbody2D>();

        }


        //prevents game from freezing if distance is zero
        if (distance == 0)
        {
            distance = 0.5f;
        }
    }
    public void OnDrawGizmos()
    {

        GUIStyle style = new GUIStyle();

        Gizmos.DrawWireSphere(base.transform.position, 0.2f);

    }
    // Update is called once per frame
    void Update()
    {


        //moves hook to desired position
        if (transform.position != (Vector3)destiny && !done)
            transform.position = Vector2.MoveTowards(transform.position, destiny, speed * Time.deltaTime);


        //while hook is not on destiny
        if ((Vector2)transform.position != destiny && !done)
        {

            //if distance from last node to rodtransform, is big enough
            if (Vector2.Distance(rodtransform.transform.position, lastNode.transform.position) > distance)
            {

                //create a node
                CreateNode();

            }

        }
        else if (done == false)
        //if node is on position and rope is not yet done
        {
            //set it to done
            done = true;
            //creates node between last node and rodtransform (in the same frame)
            while (Vector2.Distance(rodtransform.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }

            //enables joint to move with object(happens only if target is not null)
            if (hinge != null)
                hinge.autoConfigureConnectedAnchor = false;

            //binds last node to rodtransform
            lastNode.GetComponent<HingeJoint2D>().connectedBody = rodtransform.GetComponent<Rigidbody2D>();
        }
        RenderLine();

    }

    public void crankdown()
    {
        while (Vector2.Distance(rodtransform.transform.position, lastNode.transform.position) > distance)
        {
            CreateNode();
        }
    }
    //renders rope
    void RenderLine()
    {
        int i = 0;
        //sets vertex count of rope
        lr.positionCount = Nodes.Count + 1;

        //each node is a vertex oft the rope
        for (i = 0; i < Nodes.Count; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
        }

        //sets last vetex of rope to be the rodtransform
        lr.SetPosition(i, rodtransform.transform.position);

    }


    public void CreateNode()
    {
        //finds position to create and creates node (vertex)

        //makes vector that points from last node to rodtransform
        Vector2 pos2Create = rodtransform.transform.position - lastNode.transform.position;

        //makes it desired lenght
        pos2Create.Normalize();
        pos2Create *= distance;

        //adds lastnode's position to that node
        pos2Create += (Vector2)lastNode.transform.position;

        //instantiates node at that position
        GameObject go = null;
        if (Nodes.Count == 0)
        {
            go = (GameObject)Instantiate(hookPrefab, pos2Create, Quaternion.identity);
        }
        else
        {
            go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity);
        }

        //sets parent to be this hook
        go.transform.SetParent(transform);

        //sets hinge joint from last node to connect to this node
        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        //if attached to an object, turn of colliders (you may want this to be deleted in some cases)
        if (target != null && go.GetComponent<Collider2D>() != null)
        {
            go.GetComponent<Collider2D>().enabled = false;
        }


        //sets this node as the last node instantiated
        lastNode = go;

        //adds node to node list
        Nodes.Add(lastNode);
    }

    public void DestroyNode()
    {
        for (int i = 0; i < Nodes.Count - 1; i++)
        {
            Nodes[i].transform.position = new Vector3(Nodes[i+1].transform.position.x, Nodes[i+1].transform.position.y, 0);
        }

        var go = Nodes[Nodes.Count - 1];
        Nodes.Remove(go);
        Destroy(go);

        lastNode = Nodes[Nodes.Count - 1];
        Nodes[Nodes.Count - 1].GetComponent<HingeJoint2D>().connectedBody = rodtransform.GetComponent<Rigidbody2D>();

    }
}
