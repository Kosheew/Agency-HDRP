using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public interface IPatrolled
    {
        public NavMeshAgent Agent { get; }
        public Transform[] PatrolTargets { get; }
    }
}