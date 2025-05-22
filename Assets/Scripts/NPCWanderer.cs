using UnityEngine;
using UnityEngine.AI;

public class NPCWanderer : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    NavMeshAgent agent;
    [SerializeField] private float radius = 100;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        NextDestination();
    }

    private void NextDestination()
    {
        Vector3 dest = RandomNavmeshLocation(radius);
        agent.destination = dest;
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 finalPosition;

        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                break;
            }
        }

        return finalPosition;
    }

    private void Update()
    {
        anim.SetFloat(Speed, agent.velocity.magnitude / agent.speed); //0...1

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Reached
                    NextDestination();
                }
            }
        }
    }
}