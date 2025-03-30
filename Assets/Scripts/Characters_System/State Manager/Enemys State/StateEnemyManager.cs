using System.Collections.Generic;
using Characters;
using Characters.Enemy;

public class StateEnemyManager
{
    private Dictionary<EnemyContext, IEnemyState> _states = new();
    
    public IEnemyState CurrentState { get; private set; }
    
    public void SetState(IEnemyState newState, EnemyContext enemy)
    {
        if (_states.ContainsKey(enemy))
        {
            _states[enemy].ExitState(enemy);
        }

        _states[enemy] = newState;
        newState.EnterState(enemy);
    }
    
    public void UpdateState(EnemyContext enemy)
    {
        if (_states.TryGetValue(enemy, out var state))
        {
            state.UpdateState(enemy);
        }
    }
    
}
