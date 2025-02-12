using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/Event")]
public class GameEvent : ScriptableObject
{
    public UnityEvent Event;
    
    public void Invoke()
    {
        Event?.Invoke();
    }
}