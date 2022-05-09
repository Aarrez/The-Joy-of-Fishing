using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;


public sealed class RopeScript : MonoBehaviour
{
    public static RopeScript instance;
    public int currentLineLength;

    //FMOD sound
    private FMOD.Studio.EventInstance reelTickInstance;
    
    //holds where the hook is going to
    [HideInInspector] public Vector2 destiny, lastAnchorPosition;
    public Rigidbody2D hookRigidbody2D;

    //velocity that the hook goes onto the destiny
    //distance between each nodes
    [HideInInspector]public float speed = 1, distance = 0.3f, lastInstanceTimeStamp = 1, nextAllowedInstanceTimeStamp = 1;
    //required Prefabs
    public GameObject nodePrefab,hookPrefab, rodtransform, lastNode, rodTip, go;

    //line that represents rope
    public LineRenderer lr;

    public List<GameObject> Nodes = new List<GameObject>();

    //check if the full rope is created
    [HideInInspector] public bool done = false, ActualHookObject;

    //is something if an object with a rigidbody is hit
    public Transform target;

    //added hinge joint if there is relative object
    public SpringJoint2D hinge;

    PlayerScript boatScript;

    //private Transform transform;

    private void Awake()
    {
        //get sound from FMOD
        reelTickInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/reel_tick");
        //Sound at rod
        reelTickInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        
        
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
        {
            rodtransform = GameObject.FindGameObjectWithTag("PlayerRod");
            boatScript = FindObjectOfType<PlayerScript>();
            lastNode = base.transform.gameObject;
            //Nodes.Add(transform.gameObject);
        }


        //prevents game from freezing if distance is zero
        if (distance == 0)
        {
            distance = 0.3f;
        }

        if (rodTip == null)
        {
            rodTip = GameObject.Find("FishingRodTip"); // FIXME
            lastAnchorPosition = rodTip.transform.position;
        }
        
        // Define 3D attributes
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(reelTickInstance, rodtransform.transform);
    }

    void FixedUpdate() 
    {
        // move the anchor point towards the tip of the rod. Used to reel in the line
        float t = Mathf.Clamp((Time.time - lastInstanceTimeStamp) / (nextAllowedInstanceTimeStamp - lastInstanceTimeStamp), 0.0f, 1.0f);
        rodtransform.transform.position = Vector2.Lerp(lastAnchorPosition, rodTip.transform.position, t);
    }

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
            lastNode.GetComponent<SpringJoint2D>().connectedBody = rodtransform.GetComponent<Rigidbody2D>();
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
        lr.SetPosition(i, rodTip.transform.position);

    }


    public void CreateNode()
    {
        reelTickInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(rodtransform.transform));
        reelTickInstance.start(); // Play sound
    
        lastAnchorPosition = rodtransform.transform.position - new Vector3(0.05f,0.05f,0.0f);
        //finds position to create and creates node (vertex)

        //makes vector that points from last node to rodtransform
        Vector2 pos2Create = rodtransform.transform.position - lastNode.transform.position;

        //makes it desired lenght
        pos2Create.Normalize();
        pos2Create *= distance;

        //adds lastnode's position to that node
        pos2Create += (Vector2)lastNode.transform.position;

        //instantiates node at that position
        go = null;
        if (Nodes.Count == 0)
        {
            go = (GameObject)Instantiate(hookPrefab, pos2Create, Quaternion.identity);
            ActualHookObject = true; 
            hookRigidbody2D = go.GetComponent<Rigidbody2D>();
        }
        else
        {
            go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity);
            ActualHookObject = false;
        }

        //sets parent to be this hook
        go.transform.SetParent(transform);

        //sets hinge joint from last node to connect to this node
        lastNode.GetComponent<SpringJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        //if attached to an object, turn of colliders (you may want this to be deleted in some cases)
        if (target != null && go.GetComponent<Collider2D>() != null)
        {
            go.GetComponent<Collider2D>().enabled = false;
        }


        //sets this node as the last node instantiated
        lastNode = go;

        //adds node to node list
        Nodes.Add(lastNode);
        lastNode.GetComponent<SpringJoint2D>().connectedBody = rodtransform.GetComponent<Rigidbody2D>();
        currentLineLength = ((int)Nodes.Count);
    }

    public void DestroyNode()
    {
        reelTickInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(rodtransform.transform));
        reelTickInstance.start(); // Play sound
        
        List<Vector3> positions = new List<Vector3>();

        var go = Nodes.Last();
        Vector2 pos = go.transform.position;
        rodtransform.transform.position = pos;
        lastAnchorPosition = pos;
        Nodes.Remove(go);
        Destroy(go);

        GameObject node = Nodes.Last();
        lastNode = node;
        node.GetComponent<SpringJoint2D>().connectedBody = rodtransform.GetComponent<Rigidbody2D>();
        currentLineLength = ((int)Nodes.Count);

        if (Nodes.Count <= 1)
        {
            boatScript.DeleteRope();
        }

    }
}
