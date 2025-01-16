using System;
using System.Collections.Generic;

[Serializable]
public class QuestProgress
{
    public int QuestHash;
    public List<KeyValuePair<int, bool>> StepsCompleted;

    public QuestProgress(int questHash, Dictionary<int, bool> stepsCompleted)
    {
        QuestHash = questHash;
        StepsCompleted = new List<KeyValuePair<int, bool>>(stepsCompleted);
    }
    
    public Dictionary<int, bool> GetStepsCompletedDictionary()
    {
        var dictionary = new Dictionary<int, bool>();
        foreach (var pair in StepsCompleted)
        {
            dictionary[pair.Key] = pair.Value;
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