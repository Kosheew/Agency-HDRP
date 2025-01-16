using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quests;
using UnityEngine;

public class Quest 
{
    private QuestSettings _questSettings;
    public Dictionary<int, bool> StepsCompleted; // ID кроків як ключі

    public Quest(QuestSettings questSettings)
    {
        _questSettings = questSettings;
        StepsCompleted = new Dictionary<int, bool>(_questSettings.QuestStepsCount);
        InitializeSteps();
    }

    private void InitializeSteps()
    {
        foreach (var stepDescription in _questSettings.GetStepsDescriptions)
        {
            StepsCompleted.Add(stepDescription.GetHashCode(), false);
        }
    }

    public bool IsStepCompleted(int stepId)
    {
        return StepsCompleted.ContainsKey(stepId) && StepsCompleted[stepId];
    }
    

    public void CompleteStep(int stepId)
    {
        if (StepsCompleted.ContainsKey(stepId))
        {
            StepsCompleted[stepId] = true;
        }
    }

    public bool IsQuestCompleted()
    {
        return !StepsCompleted.ContainsValue(false); 
    }

    public QuestSettings GetQuestSO() => _questSettings;
    
   
}
