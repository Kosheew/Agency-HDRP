using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;  // Посилання на подію
    [SerializeField] private UnityEvent response;  // Подія, що виконується при виклику Raise()

    private void OnEnable() => gameEvent.Register(OnEventRaised);
    private void OnDisable() => gameEvent.Unregister(OnEventRaised);

    private void OnEventRaised() => response.Invoke();
}