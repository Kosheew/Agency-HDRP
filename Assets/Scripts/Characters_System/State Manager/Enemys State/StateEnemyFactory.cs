using System;
using Characters;
using Enemy.State;
using System.Collections.Generic;
using Characters.Enemy;

public class StateEnemyFactory 
{
    private readonly Dictionary<IEnemy, Dictionary<TypeEnemyStates, IEnemyState>> _statePools;

    public StateEnemyFactory(IEnemy[] enemies)
    {
        _statePools = new(enemies.Length);
        
        foreach (var enemy  in enemies)
        {
            if (!_statePools.ContainsKey(enemy))
            {
                _statePools[enemy] = new Dictionary<TypeEnemyStates, IEnemyState>
                {
                    { TypeEnemyStates.Attacked, new AttackingEnemyState() },
                    { TypeEnemyStates.Chased, new ChasingEnemyState() },
                    { TypeEnemyStates.Patrolled, new PatrollingEnemyState() },
                    { TypeEnemyStates.Idle, new IdleEnemyState() },
                    { TypeEnemyStates.Dead, new DeathEnemyState() }
                };
            }
        }
    }
    
    public IEnemyState CreateState(IEnemy enemy, TypeEnemyStates stateName)
    {
        if (_statePools.ContainsKey(enemy))
        {
            return _statePools[enemy][stateName];
        }

        throw new ArgumentException($"Unknown state: {stateName}");
    }
}
