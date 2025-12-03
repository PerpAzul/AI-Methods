using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public float chaseRange = 8f;
    public float wanderRadius = 10f;

    private Vector3 wanderTarget;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        SetNewWanderTarget();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            // Chase player
            agent.SetDestination(player.position);
        }
        else
        {
            // Wander randomly
            if (Vector3.Distance(transform.position, wanderTarget) < 1f)
                SetNewWanderTarget();

            agent.SetDestination(wanderTarget);
        }
    }

    void SetNewWanderTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, wanderRadius, 1);

        wanderTarget = hit.position;
    }
}
