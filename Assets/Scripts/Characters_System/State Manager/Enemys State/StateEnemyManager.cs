using System.Collections.Generic;
using Characters;
using Characters.Enemy;

public class StateEnemyManager
{
    private Dictionary<IEnemy, IEnemyState> _states = new();
    
    public IEnemyState CurrentState { get; private set; }
    
    public void SetState(IEnemyState newState, IEnemy enemy)
    {
        if (_states.ContainsKey(enemy))
        {
            _states[enemy].ExitState(enemy);
        }

        _states[enemy] = newState;
        newState.EnterState(enemy);
    }
    
    public void UpdateState(IEnemy enemy)
    {
        if (_states.TryGetValue(enemy, out var state))
        {
            state.UpdateState(enemy);
        }
    }
    
}
