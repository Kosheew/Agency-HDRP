using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameEventListener : MonoBehaviour
{
    [FormerlySerializedAs("gameEvent")] [SerializeField] private GameEventData gameEventData;  // Посилання на подію
    [SerializeField] private UnityEvent response;  // Подія, що виконується при виклику Raise()

    private void OnEnable() => gameEventData.Register(OnEventRaised);
    private void OnDisable() => gameEventData.Unregister(OnEventRaised);

    private void OnEventRaised() => response.Invoke();
}