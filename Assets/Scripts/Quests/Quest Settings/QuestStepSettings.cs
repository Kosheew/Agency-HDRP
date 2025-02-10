using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest Step", menuName = "Quest/QuestStepSettings")]
    public class QuestStepSettings : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string uniqueID = Guid.NewGuid().ToString();
        [TextArea(3, 10), SerializeField] private string description;

        public string Description => description;
        public string QuestName => questName;
        public string UniqueID => uniqueID;

    }
}