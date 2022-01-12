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

    private OnPathDelegate pathCallback;

    //Max distance from the Ai it can wander
    [SerializeField] private float wanderRadius = 5f;

    //The amount of random movment when moveing towards the destination
    [SerializeField] private float wanderJitter = 1f;

    private float dist = 0f;

    private void Awake()
    {
        agent = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
        player = FindObjectOfType<BaitScript>().transform;
    }

    private void OnEnable()
    {
        Wander();
    }

    private void LateUpdate()
    {
        AiMovement();
    }

    //Method that gets a random position in the world and sets the destination
    private void Wander()
    {
        wander += new Vector3(Random.Range(-1f, 1f) * wanderJitter, Random.Range(-1f, 1f) * wanderJitter);

        wander = wander.normalized;
        wander *= wanderRadius;

        Vector3 targetWorld = this.gameObject.transform.TransformVector(wander);

        Seek(targetWorld);
    }

    //Sets a position for the Ai to move towards
    private void Seek(Vector3 target)
    {
        agent.StartPath(this.transform.position, target, pathCallback);
    }

    //Does the exact opposite of Seek()
    private void Flee(Vector3 position)
    {
        Vector3 fleeVector = position - this.transform.position;
        Vector3 fleePos = this.transform.position - fleeVector;
        agent.StartPath(this.transform.position, fleePos, pathCallback);
    }

    //Main movement method
    private void AiMovement()
    {
        if (path.reachedEndOfPath)
        {
            Wander();
        }
    }

    private void HookIsOut()
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
                Seek(player.position);
            }
            else
            {
                Flee(player.position);
            }
        }

        //Just trying the ?: operator
        var x = transform.rotation.z < 0 ? base.sprRend.flipY = true : base.sprRend.flipY = false;
    }
}