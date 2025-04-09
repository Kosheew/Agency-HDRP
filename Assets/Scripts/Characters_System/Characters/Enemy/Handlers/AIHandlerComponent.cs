using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;
using UnityEngine.AI;
using CharacterSettings;
using CustomAI.Handlers;

public class AIHandlerComponent : MonoBehaviour
{
    [Header("Movement Settings")]
  
    
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolTargets;
    [SerializeField] private float _waypointThreshold = 1f;
    [SerializeField] private float _rotationSpeed = 5f;    
    [SerializeField] private float _facingAngleThreshold = 10f; 


    [Header("Chase Settings")]
    [SerializeField] private float _pathUpdateMinDistance = 0.5f; // Мінімальна відстань для оновлення шляху
    [SerializeField] private float _pathUpdateCooldown = 0.3f;
    
    [SerializeField] private float stopToTarget;
    private Vector3 _lastTargetPosition;
    private float _lastPathUpdateTime;
    
    private NavMeshAgent _agent;
    private EnemySetting _enemySetting;
        
    [SerializeField] private List<Vector3> _waypoints;
    private Vector3 _previousWaypoint;
    
    private Transform _currentTarget;
    private int _currentPatrolTargetIndex = 0;
    private int _currentWayPointIndex = 0;
    
    public bool IsMoving { get; private set; }
        
    private bool _reversePatrolDirection = false;
    
    private bool isPlayerVisible;
    
    private RotationHandler _rotationHandler;
    
        public void Init(EnemySetting enemySetting)
        {
            _waypoints = new List<Vector3>(15);
            _enemySetting = enemySetting;
            _agent = GetComponent<NavMeshAgent>();
            _rotationHandler = GetComponent<RotationHandler>();
            _agent.speed = _enemySetting.MoveSpeed; 
            _agent.updateRotation = false;
        }
        
        public void StartPatrol()
        {
            if (_patrolTargets.Length < 2) return;
            
            _currentPatrolTargetIndex = 0;
            _currentWayPointIndex = 0;
            IsMoving = true;
            CalculatePath(transform.position, _patrolTargets[_currentPatrolTargetIndex].position);
            _agent.SetDestination(_patrolTargets[_currentPatrolTargetIndex].position);
        }
        
        public void MoveTo(Transform target)
        {
            _currentTarget = target;
            _lastTargetPosition = _currentTarget.position;
            CalculatePath(transform.position, _currentTarget.position);
            
            _agent.SetDestination(_currentTarget.position);
        }
        
        public void Stop() 
        {
            _agent.isStopped = true;
            IsMoving = false;
        }

        public void Resume() 
        {
            _agent.isStopped = false;
            IsMoving = true;
        }

        public void UpdateChase()
        {
            float distanceToTarget = Vector3.Distance(_currentTarget.position, _lastTargetPosition);
            
            UpdateWayPoints();            
            
            if (distanceToTarget >= _pathUpdateMinDistance && Time.time - _lastPathUpdateTime >= _pathUpdateCooldown)
            {
                _lastTargetPosition = _currentTarget.position;
                _lastPathUpdateTime = Time.time;
                CalculatePath(transform.position, _currentTarget.position);
                _agent.SetDestination(_currentTarget.position);
            }
            
            if (_currentWayPointIndex >= _waypoints.Count) return;
            _rotationHandler.HandleRotation(_waypoints[_currentWayPointIndex]);
        }
        
        public void UpdatePatrol()
        {
            if (!IsMoving) return;

            UpdateWayPoints();

            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                CompleteCurrentPath();
                return;
            }
             
            if (_currentWayPointIndex >= _waypoints.Count) return;
            _rotationHandler.HandleRotation(_waypoints[_currentWayPointIndex]);
        }

        private void UpdateWayPoints()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, _waypoints[_currentWayPointIndex]);
            if (distanceToWaypoint <= _waypointThreshold && _currentWayPointIndex < _waypoints.Count - 1) _currentWayPointIndex++;
        }
        
        private void CompleteCurrentPath()
        {
            
            if ((!_reversePatrolDirection && _currentPatrolTargetIndex >= _patrolTargets.Length - 1) ||
                (_reversePatrolDirection && _currentPatrolTargetIndex <= 0))
            {
                _reversePatrolDirection = !_reversePatrolDirection;
            }
            
            CalculateNextPatrolPath();
        }
        
        private void CalculateNextPatrolPath()
        {
            int previousIndex = _currentPatrolTargetIndex;
            _currentPatrolTargetIndex = GetNextPatrolIndex();

            Vector3 from = _patrolTargets[previousIndex].position;
            Vector3 to = _patrolTargets[_currentPatrolTargetIndex].position;

            CalculatePath(from, to);
            _agent.SetDestination(to);
        }
        
        private int GetNextPatrolIndex()
        {
            return Mathf.Clamp(_currentPatrolTargetIndex + (_reversePatrolDirection ? -1 : 1), 0, _patrolTargets.Length - 1);
        }
        
        private void CalculatePath(Vector3 from, Vector3 to)
        {
            _waypoints.Clear();
            
            NavMeshPath tempPath = new NavMeshPath();
            
            if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, tempPath))
            {
                foreach (var point in  tempPath.corners)
                {
                    _waypoints.Add(point);
                }
            }

            _currentWayPointIndex = 0;
        }

   
        private void OnDrawGizmos()
        {
            if (_waypoints == null || _waypoints.Count == 0) return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < _waypoints.Count; i++)
            {
                Gizmos.DrawSphere(_waypoints[i], 0.2f);
                if (i < _waypoints.Count - 1)
                {
                    Gizmos.DrawLine(_waypoints[i], _waypoints[i + 1]);
                }
            }
        }

        
        public bool CheckPlayerVisibility()
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (_currentTarget.position - transform.position).normalized;
        
            Debug.DrawRay(transform.position, directionToPlayer * 10, Color.red);
        
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, 10))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log("Player is visible");
                    return true;
                }
            }
        
            return false;
        }
    
}
