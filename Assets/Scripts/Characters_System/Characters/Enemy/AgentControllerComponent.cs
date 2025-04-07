using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterSettings;

public class AgentControllerComponent : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _stuckCheckDistance = 0.1f;
    [SerializeField] private float _stuckTimeThreshold = 2f;
    [SerializeField] private float _pathUpdateFrequency = 0.5f;
    
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _patrolTargets;
    [SerializeField] private float _waypointThreshold = 1f;
    [SerializeField] private float _targetThreshold = 3f;
    [SerializeField] private float _rotationSpeed = 5f;    
    [SerializeField] private float _facingAngleThreshold = 10f; 

    [Header("Path Update Settings")]
    [SerializeField] private float _minPathUpdateDistance = 1f; 
    [SerializeField] private float _maxPathUpdateFrequency = 0.3f; 
    
    [Header("Chase Settings")]
    [SerializeField] private float _pathUpdateMinDistance = 0.5f; // Мінімальна відстань для оновлення шляху
    [SerializeField] private float _pathUpdateCooldown = 0.3f; 
    
    private Vector3 _lastTargetPosition;
    private float _lastPathUpdateTime;
    
        private NavMeshAgent _agent;
        private EnemySetting _enemySetting;
        
        private Queue<Vector3> _waypoints;
        private Vector3 _currentWaypoint;
        private Vector3 _previousWaypoint;
        
        private Transform _currentTarget;
        private int _currentPatrolTargetIndex = 0;

        public Transform[] PatrolTargets => _patrolTargets;
        public bool IsMoving { get; private set; }
        public bool IsRotating { get; private set; }
        
        private bool _reversePatrolDirection = false;
        private bool _isMovingToTarget = false;
        
        public void Init(EnemySetting enemySetting)
        {
            _waypoints = new Queue<Vector3>(15);
            _enemySetting = enemySetting;
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _enemySetting.MoveSpeed;
        }
        
        public void StartPatrol()
        {
            if (_patrolTargets.Length < 2)
            {
                Debug.LogWarning("Not enough patrol points assigned!");
                return;
            }

            _currentPatrolTargetIndex = 0;
            _isMovingToTarget = false;
            IsMoving = true;
            _waypoints = CalculatePath(transform.position, _patrolTargets[_currentPatrolTargetIndex].position);
            
            if (TryGetNextPoint(out _currentWaypoint))
            {
                _agent.SetDestination(_currentWaypoint);
            }
        }
        
        public void MoveTo(Transform target)
        {
            _isMovingToTarget = true;
            _currentTarget = target;
            _lastTargetPosition = _currentTarget.position;
            _waypoints = CalculatePath(transform.position, target.position);
            
            if (TryGetNextPoint(out _currentWaypoint))
            {
                _agent.SetDestination(_currentWaypoint);
            }
        }
        
        public void Stop() 
        {
            _agent.isStopped = true;
            IsMoving = false;
        }

        public void Resume() 
        {
            _isMovingToTarget = false;
            _agent.isStopped = false;
            IsMoving = true;
        }
        
        public void UpdateHandle()
        {
            if (!IsMoving) return;
            
            float distanceToWaypoint = Vector3.Distance(transform.position, _currentWaypoint);
            
            if (_isMovingToTarget)
            {
                HandleChaseMovement();
            }

            if (distanceToWaypoint < _waypointThreshold)
            {
                if (TryGetNextPoint(out _currentWaypoint))
                {
                    _agent.SetDestination(_currentWaypoint);
                }
                else
                {
                    CompleteCurrentPath();
                    return;
                }
            }
            
            HandleRotation();
        }
        
        private void HandleChaseMovement()
        {
            if (Time.time - _lastPathUpdateTime < _pathUpdateCooldown) return;

            float targetDisplacement = Vector3.Distance(_lastTargetPosition, _currentTarget.position);
    
            if (targetDisplacement > _pathUpdateMinDistance)
            {
                _lastTargetPosition = _currentTarget.position;
        
                // Плавне оновлення шляху - додаємо нові точки до існуючої черги
                var newPath = CalculatePath(transform.position, _lastTargetPosition);
                if (newPath.Count > 0)
                {
                    // Зберігаємо поточну відстань до наступної точки
                    float remainingDistance = _agent.remainingDistance;
            
                    // Оновлюємо чергу, зберігаючи актуальні точки
                    _waypoints = newPath;
            
                    // Якщо агент вже близько до цілі, оновлюємо точку негайно
                    if (remainingDistance < _waypointThreshold * 2f && TryGetNextPoint(out _currentWaypoint))
                    {
                        _agent.SetDestination(_currentWaypoint);
                    }
            
                    _lastPathUpdateTime = Time.time;
                }
            }
        }
        
        private void CompleteCurrentPath()
        {
            _currentPatrolTargetIndex = GetNextPatrolIndex();
        
            if ((!_reversePatrolDirection && _currentPatrolTargetIndex >= _patrolTargets.Length - 1) ||
                (_reversePatrolDirection && _currentPatrolTargetIndex <= 0))
            {
                _reversePatrolDirection = !_reversePatrolDirection;
            }
            
            if(!_isMovingToTarget)
                CalculateNextPatrolPath();
        }
        
        private void CalculateNextPatrolPath()
        {
            int nextIndex = GetNextPatrolIndex();
            _waypoints = CalculatePath(_patrolTargets[_currentPatrolTargetIndex].position, _patrolTargets[nextIndex].position);
            
            if (TryGetNextPoint(out _currentWaypoint))
            {
                _agent.SetDestination(_currentWaypoint);
            }
        }
        
        private int GetNextPatrolIndex()
        {
            return Mathf.Clamp(_currentPatrolTargetIndex + (_reversePatrolDirection ? -1 : 1), 0, _patrolTargets.Length - 1);
        }
        
        private Queue<Vector3> CalculatePath(Vector3 from, Vector3 to)
        {
            Queue<Vector3> waypoints = new Queue<Vector3>(10);
            NavMeshPath tempPath = new NavMeshPath();
            
            if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, tempPath))
            {
                foreach (var point in  tempPath.corners)
                {
                    waypoints.Enqueue(point);
                }
            }

            return waypoints;
        }

        private void HandleRotation()
        {
            Vector3 targetDirection = (_currentWaypoint - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, targetDirection);

            if (angle > _facingAngleThreshold)
            {
                IsRotating = true;
                RotateTowards(targetDirection);
            }
            else
            {
                IsRotating = false;
            }
        }
        
        private void RotateTowards(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
        
        public bool TryGetNextPoint(out Vector3 nextPoint)
        {
            if (_waypoints.Count > 0)
            {
                nextPoint = _waypoints.Dequeue();
                return true;
            }

            nextPoint = default;
            return false;
        }
}
