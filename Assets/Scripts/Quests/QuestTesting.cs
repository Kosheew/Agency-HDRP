using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestTesting : MonoBehaviour
{
    [SerializeField] private QuestSettings questSettings;

    [SerializeField] private QuestManager questManager;
    
    public void TestQuest()
    {
        int hashQuest = QuestHashUtility.GetQuestHash(questSettings.QuestName);
        int hashQuestStep = QuestHashUtility.GetQuestHash(questSettings.GetStepsDescriptions[0].QuestName);
        
        questManager.CompleteQuestStep(hashQuest, hashQuestStep);
    }
    
}
