using System;
using System.Collections.Generic;

[Serializable]
public class QuestProgress
{
    public ushort QuestHash;
    public List<ushort> StepIds; 
    public List<bool> StepCompletionStatus;

    public QuestProgress(ushort questHash, Dictionary<ushort, bool> stepsCompleted)
    {
        QuestHash = questHash;
        StepIds = new List<ushort>(stepsCompleted.Keys);
        StepCompletionStatus = new List<bool>(stepsCompleted.Values);
    }
    
    public Dictionary<ushort, bool> GetStepsCompletedDictionary()
    {
        var dictionary = new Dictionary<ushort, bool>();
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