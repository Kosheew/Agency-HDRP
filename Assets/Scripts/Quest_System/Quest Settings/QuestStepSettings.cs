using Sirenix.OdinInspector;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest Step", menuName = "ScriptableObjects/Quest/QuestStepSettings")]
    public class QuestStepSettings : ScriptableObject
    {
        [Title("Основні дані")]
        
        [BoxGroup("Основні дані")]
        [LabelText("Унікальний ID"), SerializeField]
        private ushort uniqueID;
        
        [BoxGroup("Основні дані")]
        [LabelText("Назва кроку"), SerializeField]
        private string questName;
        
        [BoxGroup("Основні дані")]
        [LabelText("Опис"), SerializeField, TextArea(3, 10)]
        private string description;

        public string Description => description;
        public string QuestName => questName;
        public ushort UniqueID => uniqueID;

    }
}