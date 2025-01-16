using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestSettings> availableQuests; 
    private Dictionary<int, Quest> _activeQuests; 
    private Dictionary<int, QuestSettings> _availableQuestLookup;
    
    private BinarySaveSystem _saveSystem;
    
    private void Start()
    {
        _saveSystem = new BinarySaveSystem();
        _availableQuestLookup = new Dictionary<int, QuestSettings>(availableQuests.Count);
    
        foreach (var questSettings in availableQuests)
        {
            int questHash = questSettings.GetHashCode();
            if (!_availableQuestLookup.ContainsKey(questHash))
            {
                _availableQuestLookup.Add(questHash, questSettings);
            }
        }
    
        _activeQuests = new Dictionary<int, Quest>(availableQuests.Count);
        
        var loadData = _saveSystem.Load<QuestProgressData>();
        if (loadData != null)
        {
            foreach (var questProgress in loadData.Quests)
            {
                if (_availableQuestLookup.TryGetValue(questProgress.QuestHash, out var questSettings))
                {
                    var quest = new Quest(questSettings);
                    quest.StepsCompleted = questProgress.GetStepsCompletedDictionary(); // Передбачається, що у Quest є цей метод
                    _activeQuests.Add(questProgress.QuestHash, quest);
                }
                else
                {
                    Debug.LogWarning($"Quest with hash {questProgress.QuestHash} not found in available quests.");
                }
            }
        }
    }

    
    public void ActivateQuest(int questHashCode)
    {
        if (_availableQuestLookup.TryGetValue(questHashCode, out var questSettings))
        {
            if (!_activeQuests.ContainsKey(questHashCode))
            {
                var newQuest = new Quest(questSettings);
                // Якщо квест новий, ініціалізуємо порожній список кроків
                newQuest.StepsCompleted = new Dictionary<int, bool>();
                _activeQuests.Add(questHashCode, newQuest);
                Debug.Log($"Quest {questSettings.QuestName} activated!");
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

    
    public void CompleteQuestStep(int questHashCode, int questStepHashCode)
    {
        if (_activeQuests.TryGetValue(questHashCode, out var quest))
        {
            quest.CompleteStep(questStepHashCode);
            if (quest.IsQuestCompleted())
            {
                Debug.Log($"Quest {quest.GetQuestSO().QuestName} completed!");
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
            int questHash = questPair.Key;
            Dictionary<int, bool> stepsCompleted = questPair.Value.StepsCompleted;
            questProgressList.Add(new QuestProgress(questHash, stepsCompleted));
        }

        var progressData = new QuestProgressData(questProgressList);
        
        _saveSystem.Save(progressData);

        Debug.Log("Quest progress saved!");
    }

}
