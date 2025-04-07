using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;

public class AILerpWithStopDistance : MonoBehaviour
{
    public float stopDistance = 2f; // Дистанція зупинки
    public float rotationSpeed = 5f; 
    private AIDestinationSetter aiDestinationSetter;
    [SerializeField] private Transform target;
    
    public float visionRange = 10f;   // Дальність зору
    public LayerMask obstacleMask;    // Шари, які вважаються перешкодами
    public float updateRate = 0.5f;   // Частота оновлення шляху (секунди)

    private Seeker seeker;
    private AILerp aiLerp;
    private float lastPathUpdate;
    private bool isPlayerVisible;

    private Rigidbody rb;
    
    void Start()
    {
        aiLerp = GetComponent<AILerp>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody>();
        seeker = GetComponent<Seeker>();
        aiDestinationSetter.target = target; // Замініть на ваш об'єкт цілі
        rotationSpeed = aiLerp.rotationSpeed;
    }

    void Update()
    {
        if (target == null) return;
        
        float distance = Vector3.Distance(transform.position, target.position);
        
        isPlayerVisible = CheckPlayerVisibility();

        if (isPlayerVisible && distance <= stopDistance)
        {
            if (!isRetreating)
            {
                aiDestinationSetter.target = target;
            }
            
            aiLerp.canMove = !(distance <= stopDistance) || isRetreating;
            // aiLerp.canMove = !isPlayerVisible;
            
            CheckForBlockingAgents();
        }
        
        if (!aiLerp.canMove)
        {
            RotateTowardsTarget();
        }
    }
    
    bool CheckPlayerVisibility()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > visionRange) return false;
        
        RaycastHit hit;
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        
        Debug.DrawRay(transform.position, directionToPlayer * visionRange, Color.red);
        
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
        {
            // Якщо Raycast влучив у гравця (немає перешкод)
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("Player is visible");
                return true;
            }
        }
        
        return false;
    }
    

    void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
    
    private Transform currentTarget;
    private Vector3 originalDestination;
    private bool isRetreating;
    
    public float avoidanceRadius = 2f;
    public float avoidanceForce = 10f;
    public float predictionTime = 0.5f;
    public float minPushForce = 1f;
    
    public List<Transform> tacticalPoints = new List<Transform>();
    
    void CheckForBlockingAgents()
    {
        if (isRetreating) return;

        Collider[] agents = Physics.OverlapSphere(transform.position, avoidanceRadius);
        foreach (var agent in agents)
        {
            if (agent.gameObject != gameObject && IsBlockingPath(agent.transform))
            {
                RetreatFromPosition(agent.transform.position);
                break;
            }
        }
    }

    bool IsBlockingPath(Transform otherAgent)
    {
        Vector3 dirToOther = (otherAgent.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToOther);
        return angle < 45f && Vector3.Distance(transform.position, otherAgent.position) < avoidanceRadius;
    }

    void RetreatFromPosition(Vector3 blockingPosition)
    {
        isRetreating = true;
        
        // 1. Знайти найближчу вільну тактичну точку
        Transform bestPoint = FindNearestTacticalPoint(blockingPosition);
        
        // 2. Або обчислити позицію відступу
        Vector3 retreatPos = bestPoint != null ? 
            bestPoint.position : 
            CalculateRetreatPosition(blockingPosition);

        // 3. Рухатись до позиції відступу
        aiLerp.destination = retreatPos;
        aiLerp.canMove = true;
        
        aiDestinationSetter.target = bestPoint;
        
        Invoke(nameof(ReturnToOriginalPath), 2f);
    }

    Transform FindNearestTacticalPoint(Vector3 fromPosition)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var point in tacticalPoints)
        {
            float dist = Vector3.Distance(fromPosition, point.position);
            if (dist < minDistance && !IsPointOccupied(point.position))
            {
                minDistance = dist;
                nearest = point;
            }
        }
        return nearest;
    }

    bool IsPointOccupied(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, 0.5f);
        return colliders.Length > 0;
    }

    Vector3 CalculateRetreatPosition(Vector3 blockingPosition)
    {
        Vector3 retreatDir = (transform.position - blockingPosition).normalized;
        return transform.position + retreatDir * 0.3f;
    }

    void ReturnToOriginalPath()
    {
        aiLerp.destination = target.position;
        aiDestinationSetter.target = target;
        
        aiLerp.canMove = !isPlayerVisible;
        isRetreating = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
        
        Gizmos.color = Color.red;
        foreach (var point in tacticalPoints)
        {
            Gizmos.DrawSphere(point.position, 0.3f);
        }
    }
}
