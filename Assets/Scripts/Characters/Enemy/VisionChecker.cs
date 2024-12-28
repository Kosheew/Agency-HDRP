using UnityEngine;
using Characters;
using Characters.Enemy;

public class VisionChecker 
{
    private bool _isTargetVisible;
    private float _loseTargetTimer;
    private float _checkTimer;
    
    private readonly RaycastHit[] _raycastHits = new RaycastHit[10]; 
       
    public bool CheckTarget(IEnemy enemy, IPlayer targetPlayer)
    { 
        _checkTimer -= Time.deltaTime;
        if (_checkTimer <= 0)
        {
            _checkTimer = enemy.EnemySetting.CheckInterval;

            if (targetPlayer == null || !targetPlayer.Alive)
            {
                _isTargetVisible = false;
            }

            if (CanSeeTarget(enemy, targetPlayer))
            {
                _isTargetVisible = true;
                _loseTargetTimer = enemy.EnemySetting.LoseTargetDelay;
            }
            else
            {
                _loseTargetTimer -= enemy.EnemySetting.CheckInterval;
                    
                if (_loseTargetTimer <= 0)
                {
                    _isTargetVisible = false;
                    targetPlayer = null;
                }
            }
        }
        return _isTargetVisible;
    }

    private bool CanSeeTarget(IEnemy enemy, IPlayer targetPlayer)
    {
        var setting = enemy.EnemySetting;
        float stepAngle = setting.StepAngle; 
        int additionalRays = Mathf.CeilToInt(setting.FieldOfViewAngle / stepAngle);

        Vector3 startDirection = Quaternion.Euler(0, -setting.FieldOfViewAngle / 2, 0) * enemy.EyesPosition.forward;

        for (int i = 0; i <= additionalRays; i++)
        {
            Vector3 currentDirection = Quaternion.Euler(0, stepAngle * i, 0) * startDirection;

            Debug.DrawRay(enemy.EyesPosition.position, currentDirection * setting.VisionDistance, Color.yellow, 0.1f);

            int hits = Physics.RaycastNonAlloc(enemy.EyesPosition.position, currentDirection, _raycastHits, setting.VisionDistance, setting.VisionMask);

            if (hits == 0) continue;
                
            for (int j = 0; j < hits; j++)
            {
                var hit = _raycastHits[j];

                if (hit.transform == targetPlayer.TransformMain)
                {
                    return IsPlayerVisible(hit, enemy.EyesPosition.position);
                }
            }
        }
        return false;
    }
    
    private bool IsPlayerVisible(RaycastHit hit, Vector3 origin)
    {
        float playerDistance = (origin - hit.point).sqrMagnitude;

        foreach (var raycastHit in _raycastHits)
        {
            if (raycastHit.transform == null) continue; 

            float otherDistance = (origin - raycastHit.point).sqrMagnitude;
            if (otherDistance < playerDistance && raycastHit.transform != hit.transform)
            { 
                return false; 
            }
        }
        return true; 
    }
}
