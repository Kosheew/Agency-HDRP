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
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolTargets;
    [SerializeField] private float _waypointThreshold = 1f;
    [SerializeField] private float _rotationSpeed = 5f;    
    [SerializeField] private float _facingAngleThreshold = 10f; 
    
    [Header("Chase Settings")]
    [SerializeField] private float _pathUpdateMinDistance = 0.5f; 
    [SerializeField] private float _pathUpdateCooldown = 0.3f;
    
    [SerializeField] private float stopToTarget;
    private Vector3 _lastTargetPosition;
    private float _lastPathUpdateTime;
    
    private DestinationHandler _destinationHandler;
    
    [SerializeField] private List<Vector3> _waypoints;
    private Vector3 _previousWaypoint;
    
    private Transform _currentTarget;
    private int _currentPatrolTargetIndex = 0;
    private int _currentWayPointIndex = 0;
    
        
    private bool _reversePatrolDirection = false;
    
    private bool isPlayerVisible;
    
    private RotationHandler _rotationHandler;
    
        public void Init(EnemySetting enemySetting)
        {
            _waypoints = new List<Vector3>(15);
            
            _rotationHandler = GetComponent<RotationHandler>();
            _destinationHandler = GetComponent<DestinationHandler>();
            _destinationHandler.Init();
        }
        
        public void StartPatrol()
        {
            if (_patrolTargets.Length < 2) return;
            
            _currentPatrolTargetIndex = 0;
            _currentWayPointIndex = 0;
            
            CalculatePath(transform.position, _patrolTargets[_currentPatrolTargetIndex].position);
            
            _destinationHandler.SetDestinationAgent(_patrolTargets[_currentPatrolTargetIndex]);
            //_agent.SetDestination(_patrolTargets[_currentPatrolTargetIndex].position);
        }
        
        public void ChaseMainTarget()
        {
            // MB problems - ?
            _destinationHandler.ResetMainDestination();

            _currentTarget = _destinationHandler.CurrentTarget;
            _lastTargetPosition = _currentTarget.position;
            CalculatePath(transform.position, _currentTarget.position);
            
            
            //_agent.SetDestination(_currentTarget.position);
        }
        
        public void Stop() 
        {
            _destinationHandler.SetAgentStopped(true);
        }

        public void Resume() 
        {
            _destinationHandler.SetAgentStopped(false);
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
                _destinationHandler.ResetMainDestination();
            }
            
            if (_currentWayPointIndex >= _waypoints.Count) return;
            _rotationHandler.HandleRotation(_waypoints[_currentWayPointIndex]);
        }
        
        public void UpdatePatrol()
        {
            UpdateWayPoints();

            if (_destinationHandler.IsDestinationReached())
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

            var from = _patrolTargets[previousIndex];
            var to = _patrolTargets[_currentPatrolTargetIndex];

            CalculatePath(from.position, to.position);
            _destinationHandler.SetDestinationAgent(to);
            //_agent.SetDestination(to);
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
        
        public bool CheckTargetVisibility(Transform target)
        {
            var directionToTarget = (target.position - transform.position).normalized;
        
            Debug.DrawRay(transform.position, directionToTarget * 10, Color.red);
        
            if (Physics.Raycast(transform.position, directionToTarget, out var hit, 10))
            {
                if (hit.transform.TryGetComponent(out PlayerContext playerContext)) return true;
            }
        
            return false;
        }
    
}
