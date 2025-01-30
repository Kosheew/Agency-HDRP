using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest Step", menuName = "Quest/QuestStepSettings")]
    public class QuestStepSettings: ScriptableObject
    {
        [SerializeField] private string questName;
        [TextArea(3, 10), SerializeField] private string description;
        
        public string Description => description;
        public string QuestName => questName;

        public override int GetHashCode()
        {
            return QuestName.GetHashCode();
        }
    }
}