using Pathfinding;
using UnityEngine;

/*
 * The fish Ai.
 * Takes care of all the fish movement.
 * Uses the AStar pathfinding prodject to get the walkable surfaces.
 */

[RequireComponent(typeof(Seeker), typeof(AIPath), typeof(FishStats))]
public class MoveAi : FishStats
{
    private Seeker agent;
    private AIPath path;
    private Transform player;

    private Vector3 wander = Vector3.zero;

    //Max distance from the Ai it can wander
    [SerializeField] private float wanderRadius = 5f;

    //The amount of random movment when moveing towards the destination
    [SerializeField] private float wanderJitter = 1f;

    private float dist = 0f;

    private bool CanFish = false;

    private void Awake()
    {
        agent = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
    }

    private void OnEnable()
    {
        Wander();
        BaitScript.BaitIsOut += delegate (bool theBait)
        {
            if (theBait)
                player = FindObjectOfType<BaitScript>().transform;
            else if (!theBait)
                player = FindObjectOfType<BoatScript>().transform;
            CanFish = theBait;
            
        };
    }

    private void LateUpdate()
    {
        if (CanFish)
        {
            HookOut();
        }
        else
        {
            HookIn();
        }
    }

    //Method that gets a random position in the world and sets the destination
    private void Wander()
    {
        wander += new Vector3(Random.Range(-1f, 1f) * wanderJitter, Random.Range(-1f, 1f) * wanderJitter);

        wander = wander.normalized;
        wander *= wanderRadius;

        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(wander);

        Seek(targetWorld);
    }

    //Sets a position for the Ai to move towards
    private void Seek(Vector3 target)
    {
        Vector3 currTarget = this.transform.position + target;
        agent.StartPath(this.transform.position, currTarget);
    }

    //Does the exact opposite of Seek()
    private void Flee(Vector3 position)
    {
        Vector3 fleeVector = position - this.transform.position;
        Vector3 fleePos = this.transform.position - fleeVector;
        agent.StartPath(this.transform.position, fleePos);
    }

    //Main movement method
    private void HookIn()
    {
        if (path.reachedEndOfPath)
        {
            Wander();
        }

        var x = transform.rotation.z < 0 ? base.sprRend.flipY = true : base.sprRend.flipY = false;
    }

    private void HookOut()
    {
        dist = Vector3.Distance(this.transform.position, player.position);
        if (dist > base.fishStats.baitAttractionRadius)
        {
            if (path.reachedEndOfPath)
            {
                Wander();
            }
        }
        if (dist < base.fishStats.baitAttractionRadius)
        {
            path.canMove = true;
            if (fishStats.baitLevel == BaitScript.BaitLevel())
            {
                agent.StartPath(this.transform.position, player.position);
            }
            else
            {
                Flee(player.position);
            }
        }

        var x = transform.rotation.z < 0 ? base.sprRend.flipY = true : base.sprRend.flipY = false;
    }
}