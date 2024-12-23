using System;
using Player.State;

namespace Characters
{
    public class StatePlayerFactory
    {
        public IPlayerState CreateState(TypeCharacterStates stateName)
        {
            return stateName switch
            {
                TypeCharacterStates.Combat => new CombatState(),
                TypeCharacterStates.Reqular => new RegularState(),
                TypeCharacterStates.Dead => new DeadState(),
                _ => throw new ArgumentException($"Unknown state: {stateName}")
            };
        }
    }
}