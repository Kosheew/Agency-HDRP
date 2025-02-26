using Quests;
using UnityEngine;

public class QuestAdder : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private QuestSettings questSettings;
    
    private QuestController _questController;

    private ushort _questCompleted;
    
    public void Inject(DependencyContainer container)
    {
        _questController = container.Resolve<QuestController>();

        _questCompleted = questSettings.UniqueID;
    }

    public void AddQuest()
    {
        _questController.ActivateQuest(_questCompleted);
    }
}
