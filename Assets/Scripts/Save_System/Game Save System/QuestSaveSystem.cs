using System.Collections.Generic;

public class QuestSaveSystem : ISaveable
{
    private QuestController _questController;

    public QuestSaveSystem(QuestController questController)
    {
        _questController = questController;
    }

    public void SaveTo(GameData gameData)
    {
        /*var questProgressList = new List<QuestProgress>();
        
        foreach (var questPair in _questController.GetQuests)
        {
            ushort questHash = questPair.Key;
            Dictionary<ushort, bool> stepsCompleted = questPair.Value.StepsCompleted;
            questProgressList.Add(new QuestProgress(questHash, stepsCompleted));
        }

        gameData.questProgressData = new QuestProgressData(questProgressList);*/
    }

    public void LoadFrom(GameData gameData)
    {
        if (gameData.questProgressData == null) return;

        foreach (var questData in gameData.questProgressData.Quests)
        {
            _questController.RestoreQuestProgress(questData);
        }
    }
}