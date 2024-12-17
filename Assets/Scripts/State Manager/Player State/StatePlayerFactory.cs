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
                TypeCharacterStates.Move => new MovementState(),
                TypeCharacterStates.Dead => new DeadState(),
                _ => throw new ArgumentException($"Unknown state: {stateName}")
            };
        }
    }
}