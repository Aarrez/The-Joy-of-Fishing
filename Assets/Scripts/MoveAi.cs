using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveAi : MonoBehaviour
{
    private Seeker agent;
    private AIPath path;
    private Transform player;

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

    private Vector3 wander = Vector3.zero;

    [SerializeField] private float wanderRadius = 10f;

    [SerializeField] private float wanderDist = 15f;
    [SerializeField] private float wanderJitter = 1f;

    private void Wander()
    {
        wander += new Vector3(Random.Range(-1f, 1f) * wanderJitter, Random.Range(-1f, 1f) * wanderJitter);

        wander = wander.normalized;
        wander *= wanderRadius;

        Vector3 targetLocal = wander + new Vector3(0f, 0f, wanderDist);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    private void Seek(Vector3 target)
    {
        agent.StartPath(this.transform.position, target);
    }

    private void Flee(Vector3 position)
    {
        Vector3 fleeVector = position - this.transform.position;
        Vector3 fleePos = this.transform.position - fleeVector;
        agent.StartPath(this.transform.position, this.transform.position - fleeVector);
    }

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
            Flee(player.position);
        }
    }
}