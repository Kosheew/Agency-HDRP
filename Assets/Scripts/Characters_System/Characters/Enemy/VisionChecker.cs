using System.Linq;
using UnityEngine;
using Characters.Character_Interfaces;
using Characters.Enemy;

public class VisionChecker 
{
    private bool _isTargetVisible;
    private float _loseTargetTimer;
    private float _checkTimer;
    
    private ITargetHandler _targetHandler;
    
    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];

    public VisionChecker(float loseTargetTimer)
    {
        _loseTargetTimer = loseTargetTimer;
    }
    public ITargetHandler CheckTarget(EnemyContext enemy)
    {
        if (!enemy.ShouldCheckTarget) return null;

        _checkTimer -= Time.deltaTime;
        if (_checkTimer <= 0) 
        {
            CanSeeTarget(enemy);
            _checkTimer = enemy.EnemySetting.CheckInterval;

            if (_targetHandler != null)
            {
                if (!_targetHandler.TargetAlive)
                {
                    _isTargetVisible = false;
                    _targetHandler = null;
                }

                if (_isTargetVisible)
                {
                    _loseTargetTimer = enemy.EnemySetting.LoseTargetDelay;
                }
                else
                {
                    _loseTargetTimer -= enemy.EnemySetting.CheckInterval;

                    if (_loseTargetTimer <= 0) _targetHandler = null;
                }
            }
        }
        return _targetHandler;
    }

    public void SetTargetHandler(ITargetHandler handler)
    {
        _targetHandler = handler;
    }
    private void CanSeeTarget(EnemyContext enemy)
    {
        var setting = enemy.EnemySetting;
        float stepAngle = setting.StepRayAngle; 
        int additionalRays = Mathf.CeilToInt(setting.FieldOfViewAngle / stepAngle);

        Vector3 startDirection = Quaternion.Euler(0, -setting.FieldOfViewAngle / 2, 0) * enemy.EyesPosition.forward;
        
        bool targetIsVisible = false; 
        
        for (int i = 0; i <= additionalRays; i++)
        {
            Vector3 currentDirection = Quaternion.Euler(0, stepAngle * i, 0) * startDirection;

            Debug.DrawRay(enemy.EyesPosition.position, currentDirection * setting.VisionDistance, Color.yellow, 0.1f);

            int hits = Physics.RaycastNonAlloc(enemy.EyesPosition.position, currentDirection, _raycastHits, setting.VisionDistance, setting.VisionMask);

            if (hits > 0)
            {
                var closestHit = _raycastHits.Take(hits)
                    .OrderBy(hit => hit.distance)
                    .First();

                if (closestHit.transform.TryGetComponent<ITargetHandler>(out var handler))
                {
                    targetIsVisible = true;
                    _targetHandler = handler;
                  //  Debug.Log($"Target detected: {closestHit.transform.name}");
                    break;
                }
                else
                {
                  //  Debug.Log($"Obstacle detected: {closestHit.transform.name}");
                    targetIsVisible = false;
                }
            }
        }
        _isTargetVisible = targetIsVisible;
    }
}
