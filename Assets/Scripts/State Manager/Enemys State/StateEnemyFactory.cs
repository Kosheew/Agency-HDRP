using System;
using Characters;
using Enemy.State;

public class StateEnemyFactory 
{
    public IEnemyState CreateState(TypeCharacterStates stateName)
    {
        return stateName switch
        {
            TypeCharacterStates.Attacked => new AttackingEnemyState(),
            TypeCharacterStates.Chased => new ChasingEnemyState(),
            TypeCharacterStates.Patrolled => new PatrollingEnemyState(),
            TypeCharacterStates.Idle => new IdleEnemyState(),
            _ => throw new ArgumentException($"Unknown state: {stateName}")
        };
    }
}
