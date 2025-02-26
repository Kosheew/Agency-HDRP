using Quests;
using UnityEngine;

public class QuestCompleter : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    [Header("Quests Step Complete")]
    [SerializeField] private QuestStepSettings questStepSettings;
    
    private QuestController _questController;

    private ushort _questCompleted;
    private ushort _questStepCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questController = container.Resolve<QuestController>();

        _questCompleted = questSettings.UniqueID;
        _questStepCompleted = questStepSettings.UniqueID;
    }

    public void CompleteStepQuest()
    {
        _questController.CompleteQuestStep(_questCompleted, _questStepCompleted);
    }
}
