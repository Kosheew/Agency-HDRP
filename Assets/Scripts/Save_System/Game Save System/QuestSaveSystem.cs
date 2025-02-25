using System.Collections.Generic;

public class QuestSaveSystem : ISaveable
{
    private QuestManager _questManager;

    public QuestSaveSystem(QuestManager questManager)
    {
        _questManager = questManager;
    }

    public void SaveTo(GameData gameData)
    {
        var questProgressList = new List<QuestProgress>();
        
        foreach (var questPair in _questManager.GetQuests)
        {
            ushort questHash = questPair.Key;
            Dictionary<ushort, bool> stepsCompleted = questPair.Value.StepsCompleted;
            questProgressList.Add(new QuestProgress(questHash, stepsCompleted));
        }

        gameData.questProgressData = new QuestProgressData(questProgressList);
    }

    public void LoadFrom(GameData gameData)
    {
        if (gameData.questProgressData == null) return;

        foreach (var questData in gameData.questProgressData.Quests)
        {
            _questManager.RestoreQuestProgress(questData);
        }
    }
}