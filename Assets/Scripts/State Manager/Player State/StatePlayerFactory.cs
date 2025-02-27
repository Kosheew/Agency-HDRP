using System;
using Player.State;
using System.Collections.Generic;

namespace Characters
{
    public class StatePlayerFactory
    {
        
        private readonly Dictionary<TypePlayerStates, IPlayerState> _statePool = new Dictionary<TypePlayerStates, IPlayerState>();

        public StatePlayerFactory()
        {
            _statePool[TypePlayerStates.Combat] = new CombatState();
            _statePool[TypePlayerStates.Regular] = new RegularState();
            _statePool[TypePlayerStates.Dead] = new DeadState();
            _statePool[TypePlayerStates.Base] = new BasePlayerState();
        }

        
        public IPlayerState GetState(TypePlayerStates stateName)
        {
            if (_statePool.ContainsKey(stateName))
            {
                return _statePool[stateName];
            }

            throw new ArgumentException($"Unknown state: {stateName}");
        }
    }
}