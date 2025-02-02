using Quests;
using UnityEngine;

public class QuestAdder : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    
    private QuestManager _questManager;

    private int _questCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questManager = container.Resolve<QuestManager>();

        _questCompleted = QuestHashUtility.GetQuestHash(questSettings.QuestName);
    }

    public void AddQuest()
    {
        _questManager.ActivateQuest(_questCompleted);
    }
}
