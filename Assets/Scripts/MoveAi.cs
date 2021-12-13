using Pathfinding;
using UnityEngine;

/*
 * The fish Ai.
 * Takes care of all the fish movement.
 * Uses the AStar pathfinding prodject to get the walkable surfaces.
 */

[RequireComponent(typeof(Seeker), typeof(AIPath))]

public class MoveAi : MonoBehaviour
{
    private Seeker agent;
    private AIPath path;
    private Transform player;
    private SpriteRenderer sprRend;

    private Vector3 wander = Vector3.zero;

    //Max distance from the Ai it can wander
    [SerializeField] private float wanderRadius = 5f;

    //The amount of random movment when moveing towards the destination
    [SerializeField] private float wanderJitter = 1f;

    public Fish fishStats;

    private float dist = 0f;

    private void Awake()
    {
        agent = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
        player = FindObjectOfType<BaitScript>().transform;
        sprRend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Wander();
        SetFishStats();
    }

    private void LateUpdate()
    {
        AiMovement();
    }

    private void SetFishStats()
    {
        sprRend.sprite = fishStats.sprite;

        sprRend.color = fishStats.fishColor;

        GetComponentInChildren<Animator>().runtimeAnimatorController = fishStats.animatorController;
        GetComponentInChildren<Animator>().SetBool("Moveing", true);
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
        agent.StartPath(this.transform.position, target);
    }

    //Does the exact opposite of Seek()
    private void Flee(Vector3 position)
    {
        Vector3 fleeVector = position - this.transform.position;
        Vector3 fleePos = this.transform.position - fleeVector;
        agent.StartPath(this.transform.position, fleePos);
    }

    //Main movement method
    private void AiMovement()
    {
        dist = Vector3.Distance(this.transform.position, player.position);
        if (dist > fishStats.baitAttractionRadius)
        {
            if (path.reachedEndOfPath)
            {
                Wander();
            }
        }
        if (dist < fishStats.baitAttractionRadius)
        {
            path.canMove = true;
            Seek(player.position);
        }

        //Just trying the ?: operator
        var x = transform.rotation.z < 0 ? sprRend.flipY = true : sprRend.flipY = false;
    }
}