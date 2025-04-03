using UnityEngine;
namespace Characters.Character_Interfaces
{
    public interface ITargetHandler
    {
        public bool TargetAlive { get;}
        Transform TargetPosition { get; set; }
    }
}