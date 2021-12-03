using UnityEngine;
using Pathfinding;

public class MoveAi : MonoBehaviour
{
    private Seeker agent;
    private AIPath path;
    private Transform player;

    private Vector3 wander = Vector3.zero;

    //Max distance from the Ai it can wander
    [SerializeField] private float wanderRadius = 10f;

    //The amount of random movment when moveing towards the destination
    [SerializeField] private float wanderJitter = 1f;

    [SerializeField] private Fish fishStats;

    private float dist = 0f;

    private void Awake()
    {
        agent = GetComponent<Seeker>();
        path = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
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
        if (dist > 5)
        {
            if (path.reachedEndOfPath)
            {
                Wander();
            }
        }
        if (dist < 5)
        {
            path.canMove = true;
            Seek(player.position);
        }
    }
}