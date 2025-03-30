using System;
using Characters;
using Enemy.State;
using System.Collections.Generic;
using Characters.Enemy;

public class StateEnemyFactory 
{
    private readonly Dictionary<EnemyContext, Dictionary<TypeEnemyStates, IEnemyState>> _statePools;

    public StateEnemyFactory(EnemyContext[] enemies)
    {
        _statePools = new(enemies.Length);
        
        foreach (var enemy  in enemies)
        {
            if (!_statePools.ContainsKey(enemy))
            {
                _statePools[enemy] = new Dictionary<TypeEnemyStates, IEnemyState>
                {
                    { TypeEnemyStates.Attacked, new AttackEnemyState() },
                    { TypeEnemyStates.Chased, new ChaseEnemyState() },
                    { TypeEnemyStates.Patrolled, new PatrolEnemyState() },
                    { TypeEnemyStates.Idle, new IdleEnemyState() },
                    { TypeEnemyStates.Dead, new DeathEnemyState() }
                };
            }
        }
    }
    
    public IEnemyState CreateState(EnemyContext enemy, TypeEnemyStates stateName)
    {
        if (_statePools.ContainsKey(enemy))
        {
            return _statePools[enemy][stateName];
        }

        throw new ArgumentException($"Unknown state: {stateName}");
    }
}
