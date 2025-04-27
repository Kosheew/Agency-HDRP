using UnityEngine;
using UnityEngine.AI;

namespace CustomAI.Handlers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DestinationHandler : MonoBehaviour
    {
        [SerializeField] private Transform mainTarget;

        [SerializeField] private bool isUpdateRotation;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private float speed;
        private NavMeshAgent _agent;
        
        public Transform CurrentTarget { get; private set; }
        
        public void Init()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = isUpdateRotation;
            _agent.stoppingDistance = stoppingDistance;
            _agent.speed = speed;
        }

        public void SetDestinationAgent(Transform target)
        {
            CurrentTarget = target;
            _agent.SetDestination(target.position);
        }

        public void ResetMainDestination()
        {
            SetDestinationAgent(mainTarget);
        }

        public void SetAgentStopped(bool isStop)
        {
            _agent.isStopped = isStop;
        }

        public bool IsDestinationReached()
        {
            return _agent.remainingDistance <= _agent.stoppingDistance;
        }
    }
}