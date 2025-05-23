using UnityEngine;
using UnityEngine.AI;

public class AgentMover : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Strafe = Animator.StringToHash("Strafe");

    private NavMeshAgent _agent;

    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask mask;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleClickMovement();
        HandleAnimation();
    }

    private void HandleClickMovement()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask))
            {
                _agent.destination = hit.point;
            }
        }
    }

    private void HandleAnimation()
    {
        if (_agent.hasPath)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);
            float forwardSpeed = localVelocity.z / _agent.speed;
            float strafeSpeed = localVelocity.x / _agent.speed;

            _animator.SetFloat(Speed, forwardSpeed);
            _animator.SetFloat(Strafe, strafeSpeed);
        }
        else
        {
            _animator.SetFloat(Speed, 0);
            _animator.SetFloat(Strafe, 0);
        }
    }
}
