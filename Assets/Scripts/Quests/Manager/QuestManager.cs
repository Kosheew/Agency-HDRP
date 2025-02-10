using System;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Start Clear")]
    [SerializeField] private bool startClear = false;
    
    [SerializeField] private QuestView questView;
    [SerializeField] private List<QuestSettings> availableQuests; 
    
    private Dictionary<string, Quest> _activeQuests; 
    private Dictionary<string, QuestSettings> _availableQuestLookup;
    private Dictionary<string, Quest> _completedQuests;
    
    private BinarySaveSystem _saveSystem;
    
    private string _startQuestHash;
    
    public void Inject(DependencyContainer container)
    {
        _saveSystem = container.Resolve<BinarySaveSystem>();

        _availableQuestLookup = new Dictionary<string, QuestSettings>(availableQuests.Count);
        _activeQuests = new Dictionary<string, Quest>(availableQuests.Count);
        _completedQuests = new Dictionary<string, Quest>(availableQuests.Count);

        if (availableQuests.Count > 0)
        {
            _startQuestHash = availableQuests[0].UniqueID;
        }
        else
        {
            Debug.LogWarning("No available quests found!");
            return;
        }

        if (startClear)
            _saveSystem.ClearSaveData();

        foreach (var questSettings in availableQuests)
        {
            var questHash = questSettings.UniqueID;
            if (!_availableQuestLookup.ContainsKey(questHash))
            {
                _availableQuestLookup.Add(questHash, questSettings);
            }
        }

        if (_saveSystem.CheckFileExists())
        {
            var loadData = _saveSystem.Load<QuestProgressData>();

            foreach (var questProgress in loadData.Quests)
            {
                if (_availableQuestLookup.TryGetValue(questProgress.QuestHash, out var questSettings))
                {
                    var quest = new Quest(questSettings);
                    quest.StepsCompleted = questProgress.GetStepsCompletedDictionary();
                    _activeQuests.Add(questProgress.QuestHash, quest);

                    if (!quest.IsQuestCompleted())
                    {
                        questView.SetQuest(quest);
                    }
                }
                else
                {
                    Debug.LogWarning($"Quest with hash {questProgress.QuestHash} not found in available quests.");
                }
            }
        }
        else
        {
            ActivateQuest(_startQuestHash);
        }
    }

    private void Start()
    {
        questView.UpdateQuests();
    }

    public void ActivateQuest(string questHashCode)
    {
        if (_availableQuestLookup.TryGetValue(questHashCode, out var questSettings))
        {
            if (!_activeQuests.ContainsKey(questHashCode))
            {
                var newQuest = new Quest(questSettings);
                
                _activeQuests.Add(questHashCode, newQuest);
                Debug.Log($"Quest {questSettings.QuestName} activated!");
                questView.SetQuest(newQuest);
            }
            else
            {
                Debug.LogWarning($"Quest {questSettings.QuestName} is already active!");
            }
        }
        else
        {
            Debug.LogWarning($"Quest with hash {questHashCode} not found in available quests!");
        }
    }
    
    public void CompleteQuestStep(string questHashCode, string questStepHashCode)
    {
        if (_activeQuests.TryGetValue(questHashCode, out var quest))
        {
            quest.CompleteStep(questStepHashCode);
            questView.SetQuest(quest);
            
            if (quest.IsQuestCompleted())
            {
                _activeQuests.Remove(questHashCode);
                _completedQuests.Add(questHashCode, quest);
                Debug.Log($"Quest {quest.GetQuestSO().QuestName} completed!");
                
                questView.UpdateQuests();
            }
        }
        else
        {
            Debug.LogWarning($"Quest with hash {questHashCode} not found!");
        }
    }
    
    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(_activeQuests.Values);
    }

    public List<Quest> GetCompletedQuests()
    {
        return new List<Quest>(_completedQuests.Values);
    }

    /// <summary>
    /// Отримує список доступних, але ще неактивних квестів.
    /// </summary>
    public List<QuestSettings> GetAvailableQuests()
    {
        var availableQuests = new List<QuestSettings>();

        foreach (var questPair in _availableQuestLookup)
        {
            if (!_activeQuests.ContainsKey(questPair.Key))
            {
                availableQuests.Add(questPair.Value);
            }
        }

        return availableQuests;
    }
    
    public void SaveQuestProgress()
    {
        var questProgressList = new List<QuestProgress>();

        foreach (var questPair in _activeQuests)
        {
            string questHash = questPair.Key;
            Dictionary<string, bool> stepsCompleted = questPair.Value.StepsCompleted;
            questProgressList.Add(new QuestProgress(questHash, stepsCompleted));
        }

        var progressData = new QuestProgressData(questProgressList);
        
        _saveSystem.Save(progressData);

        Debug.Log("Quest progress saved!");
    }
}
