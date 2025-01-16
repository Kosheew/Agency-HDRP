using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/QuestSettings")]
    public class QuestSettings: ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string questDescription;
        [SerializeField] private List<QuestStepSettings> stepsDescriptions; 

        public string QuestName => questName;
        public string QuestDescription => questDescription;
        public List<QuestStepSettings> GetStepsDescriptions => stepsDescriptions;
        public int QuestStepsCount => stepsDescriptions.Count;

        public override int GetHashCode()
        {
            return QuestName.GetHashCode();
        }
    }
}