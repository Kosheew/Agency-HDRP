using System.Collections.Generic;
using UnityEngine;

public class QuestPresenter 
{
    private List<Quest> _quests;
    private QuestController _questController;
    private QuestView _questView;
    private int _currentQuestIndex = 0;

    public QuestPresenter(DependencyContainer container)
    {
        _questController = container.Resolve<QuestController>();
        _questView = container.Resolve<QuestView>();

        _quests = _questController.GetActiveQuests();

        if (_quests.Count > 0)
        {
            _questView.SetQuest(_quests[_currentQuestIndex]);
        }
    }

    public void NextQuest()
    {
        _quests = _questController.GetActiveQuests();
        if (_quests.Count == 0) return;

        _currentQuestIndex = (_currentQuestIndex + 1) % _quests.Count;
        _questView.SetQuest(_quests[_currentQuestIndex]);
    }

    public void PreviousQuest()
    {
        _quests = _questController.GetActiveQuests();
        if (_quests.Count == 0) return;

        _currentQuestIndex = (_currentQuestIndex - 1 + _quests.Count) % _quests.Count;
        _questView.SetQuest(_quests[_currentQuestIndex]);
    }
    
    public void RefreshQuests()
    {
        _quests = _questController.GetActiveQuests();
        
        if (_quests.Count > 0)
        {
            _currentQuestIndex = Mathf.Clamp(_currentQuestIndex, 0, _quests.Count - 1);
            _questView.SetQuest(_quests[_currentQuestIndex]);
        }
        else
        {
            // Якщо активних квестів немає, очистити UI
            _questView.Clear();
        }
    }
}
