using System;
using System.Collections.Generic;

[Serializable]
public class QuestProgress
{
    public int QuestHash;
    public List<int> StepIds; 
    public List<bool> StepCompletionStatus;

    public QuestProgress(int questHash, Dictionary<int, bool> stepsCompleted)
    {
        QuestHash = questHash;
        StepIds = new List<int>(stepsCompleted.Keys);
        StepCompletionStatus = new List<bool>(stepsCompleted.Values);
    }
    
    public Dictionary<int, bool> GetStepsCompletedDictionary()
    {
        var dictionary = new Dictionary<int, bool>();
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