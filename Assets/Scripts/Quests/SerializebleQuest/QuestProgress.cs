using System;
using System.Collections.Generic;

[Serializable]
public class QuestProgress
{
    public string QuestHash;
    public List<string> StepIds; 
    public List<bool> StepCompletionStatus;

    public QuestProgress(string questHash, Dictionary<string, bool> stepsCompleted)
    {
        QuestHash = questHash;
        StepIds = new List<string>(stepsCompleted.Keys);
        StepCompletionStatus = new List<bool>(stepsCompleted.Values);
    }
    
    public Dictionary<string, bool> GetStepsCompletedDictionary()
    {
        var dictionary = new Dictionary<string, bool>();
        for (int i = 0; i < StepIds.Count; i++)
        {
            dictionary[StepIds[i]] = StepCompletionStatus[i];
        }
        return dictionary;
    }
}


[Serializable]
public class QuestProgressData
{
    public List<QuestProgress> Quests;

    public QuestProgressData(List<QuestProgress> quests)
    {
        Quests = quests;
    }
}