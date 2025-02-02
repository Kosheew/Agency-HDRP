using System.Collections.Generic;

public class QuestPresenter 
{
    private List<Quest> _quests;
    private QuestManager _questManager;
    private QuestView _questView;
    private int _currentQuestIndex = 0;

    public QuestPresenter(DependencyContainer container)
    {
        _questManager = container.Resolve<QuestManager>();
        _questView = container.Resolve<QuestView>();

        _quests = _questManager.GetActiveQuests();

        if (_quests.Count > 0)
        {
            _questView.SetQuest(_quests[_currentQuestIndex]);
        }
    }

    public void NextQuest()
    {
        _quests = _questManager.GetActiveQuests();
        if (_quests.Count == 0) return;

        _currentQuestIndex = (_currentQuestIndex + 1) % _quests.Count;
        _questView.SetQuest(_quests[_currentQuestIndex]);
    }

    public void PreviousQuest()
    {
        _quests = _questManager.GetActiveQuests();
        if (_quests.Count == 0) return;

        _currentQuestIndex = (_currentQuestIndex - 1 + _quests.Count) % _quests.Count;
        _questView.SetQuest(_quests[_currentQuestIndex]);
    }
}
