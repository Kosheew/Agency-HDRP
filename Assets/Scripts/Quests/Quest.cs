using System.Collections.Generic;
using Quests;

public class Quest 
{
    public QuestSettings QuestSettings;
    public Dictionary<int, bool> StepsCompleted; // ID кроків як ключі

    public Quest(QuestSettings questSettings)
    {
        QuestSettings = questSettings;
        StepsCompleted = new Dictionary<int, bool>(QuestSettings.QuestStepsCount);
        InitializeSteps();
    }

    private void InitializeSteps()
    {
        foreach (var stepDescription in QuestSettings.GetStepsDescriptions)
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
        StepsCompleted[stepId] = true;
    }

    public bool IsQuestCompleted()
    {
        return !StepsCompleted.ContainsValue(false); 
    }

    public QuestSettings GetQuestSO() => QuestSettings;
    
   
}
