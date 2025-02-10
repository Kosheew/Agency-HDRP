using Quests;
using UnityEngine;

public class QuestCompleter : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    [Header("Quests Step Complete")]
    [SerializeField] private QuestStepSettings questStepSettings;
    
    private QuestManager _questManager;

    private string _questCompleted;
    private string _questStepCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questManager = container.Resolve<QuestManager>();

        _questCompleted = questSettings.UniqueID;
        _questStepCompleted = questStepSettings.UniqueID;
    }

    public void CompleteStepQuest()
    {
        _questManager.CompleteQuestStep(_questCompleted, _questStepCompleted);
    }
}
