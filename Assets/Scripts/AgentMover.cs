using UnityEngine;
using UnityEngine.AI;

public class AgentMover : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private NavMeshAgent _agent;

    //[SerializeField] Transform[] targets;

    [SerializeField] LayerMask mask;

    private int index = 0;

    [SerializeField] private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        //_agent.SetDestination(targets[index].position);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                _agent.destination = hit.point;
            }
        }

        float normalizeSpeed = _agent.velocity.magnitude / _agent.speed;

        if (_agent.hasPath)
        {
            _animator.SetFloat(Speed, normalizeSpeed);
        }
        else
        {
            _animator.SetFloat(Speed, 0);
        }

        /*if (_agent.remainingDistance <= 0.1f)
        {
            index++;
            if (index >= targets.Length)
            {
                index = 0;
            }
            _agent.SetDestination(targets[index].position);
        }*/
    }
}
