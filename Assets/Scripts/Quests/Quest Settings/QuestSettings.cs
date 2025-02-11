using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/QuestSettings")]
    public class QuestSettings: ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private ushort uniqueID;
        [TextArea(3, 10), SerializeField] private string questDescription;
        [SerializeField] private List<QuestStepSettings> stepsDescriptions; 

        public string QuestName => questName;
        public string QuestDescription => questDescription;
        public List<QuestStepSettings> GetStepsDescriptions => stepsDescriptions;
        public int QuestStepsCount => stepsDescriptions.Count;
        
        public ushort UniqueID => uniqueID;
    }
}