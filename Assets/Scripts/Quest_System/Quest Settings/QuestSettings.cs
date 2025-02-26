using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest/QuestSettings")]
    public class QuestSettings: ScriptableObject
    {
        [Title("Основні дані")]
        
        [BoxGroup("Основні дані")]
        [LabelText("Унікальний ID"), SerializeField]
        private ushort uniqueID;
        
        [BoxGroup("Основні дані")]
        [LabelText("Назва квеста"), SerializeField]
        private string questName;
        
        [BoxGroup("Основні дані")]
        [LabelText("Опис квеста"), SerializeField, TextArea(3, 10)]
        private string questDescription;
        
        [Space(10)]
        [BoxGroup("Основні дані"), LabelText("Кроки квеста"), SerializeField] 
        private List<QuestStepSettings> stepsDescriptions; 

        public string QuestName => questName;
        public string QuestDescription => questDescription;
        public List<QuestStepSettings> GetStepsDescriptions => stepsDescriptions;
        public int QuestStepsCount => stepsDescriptions.Count;
        public ushort UniqueID => uniqueID;
    }
}