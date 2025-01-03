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
    
    
    public ITargetHandler CheckTarget(IEnemy enemy)
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

    private void CanSeeTarget(IEnemy enemy)
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
            
            for (int j = 0; j < hits; j++)
            {
                var hit = _raycastHits[j];

                if (hit.transform.TryGetComponent<ITargetHandler>(out var handler))
                {
                    targetIsVisible = true;
                    _targetHandler = handler;
                    Debug.Log(hit.transform.name);
                    break;
                }
                else 
                {
                    Debug.Log(hit.transform.name);
                }
                
            }
            if (targetIsVisible) break;
        }
        _isTargetVisible = targetIsVisible;
    }
}
