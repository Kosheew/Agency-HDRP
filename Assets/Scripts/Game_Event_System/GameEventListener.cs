using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEventData gameEventData;  
    [SerializeField] private UnityEvent response;  

    private void OnEnable() => gameEventData.Register(OnEventRaised);
    private void OnDisable() => gameEventData.Unregister(OnEventRaised);

    private void OnEventRaised() => response.Invoke();
}