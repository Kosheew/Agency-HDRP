using Quests;
using UnityEngine;

public class QuestCompleter : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    [Header("Quests Step Complete")]
    [SerializeField] private QuestStepSettings questStepSettings;
    
    private QuestManager _questManager;

    private int _questCompleted;
    private int _questStepCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questManager = container.Resolve<QuestManager>();

        _questCompleted = QuestHashUtility.GetQuestHash(questSettings.QuestName);
        _questStepCompleted = QuestHashUtility.GetQuestHash(questStepSettings.QuestName);
    }

    public void CompleteStepQuest()
    {
        _questManager.CompleteQuestStep(_questCompleted, _questStepCompleted);
    }
}
