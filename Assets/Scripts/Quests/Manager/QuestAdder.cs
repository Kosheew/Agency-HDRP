using Quests;
using UnityEngine;

public class QuestAdder : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    
    private QuestManager _questManager;

    private string _questCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questManager = container.Resolve<QuestManager>();

        _questCompleted = questSettings.UniqueID;
    }

    public void AddQuest()
    {
        _questManager.ActivateQuest(_questCompleted);
    }
}
