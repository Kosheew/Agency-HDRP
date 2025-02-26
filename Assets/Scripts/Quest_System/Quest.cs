using System.Collections.Generic;
using Quests;

public class Quest 
{
    public QuestSettings QuestSettings;
    public Dictionary<ushort, bool> StepsCompleted; // ID кроків як ключі

    public Quest(QuestSettings questSettings)
    {
        QuestSettings = questSettings;
        StepsCompleted = new Dictionary<ushort, bool>(QuestSettings.QuestStepsCount);
        InitializeSteps();
    }

    private void InitializeSteps()
    {
        foreach (var stepDescription in QuestSettings.GetStepsDescriptions)
        {
            StepsCompleted.Add(stepDescription.UniqueID, false);
        }
    }

    public bool IsStepCompleted(ushort stepId)
    {
        return StepsCompleted.ContainsKey(stepId) && StepsCompleted[stepId];
    }
    

    public void CompleteStep(ushort stepId)
    {
        StepsCompleted[stepId] = true;
    }

    public bool IsQuestCompleted()
    {
        return !StepsCompleted.ContainsValue(false); 
    }

    public QuestSettings GetQuestSO() => QuestSettings;
}
