using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Game/Event")]
public class GameEvent : ScriptableObject
{
    [SerializeField] private short uniqueID;
    
    private event UnityAction _listeners;

    public short UniqueID => uniqueID;
    
    public void Raise() => _listeners?.Invoke();
    
    public void Register(UnityAction listener) => _listeners += listener;
    
    public void Unregister(UnityAction listener) => _listeners -= listener;
}