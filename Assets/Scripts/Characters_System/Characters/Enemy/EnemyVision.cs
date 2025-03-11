using UnityEngine;
using CharacterSettings;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform eyesPosition;
    [SerializeField] private EnemySetting setting;
    
    private void OnDrawGizmosSelected()
    {
        float visionDistance = setting.VisionDistance;
        float fieldOfViewAngle = setting.FieldOfViewAngle;
        
        if (eyesPosition == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eyesPosition.position, Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * eyesPosition.forward * visionDistance);
        Gizmos.DrawRay(eyesPosition.position, Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * eyesPosition.forward * visionDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(eyesPosition.position, visionDistance);
    }
}