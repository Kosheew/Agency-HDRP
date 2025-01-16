using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestStepSettings")]
    public class QuestStepSettings: ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string description;
        
        public string Description => description;
        public string QuestName => questName;

        public override int GetHashCode()
        {
            return QuestName.GetHashCode();
        }
    }
}