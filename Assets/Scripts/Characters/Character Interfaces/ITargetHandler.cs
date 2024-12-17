using UnityEngine;
namespace Characters.Character_Interfaces
{
    public interface ITargetHandler
    {
        IPlayer TargetPlayer { get; }
    }
}