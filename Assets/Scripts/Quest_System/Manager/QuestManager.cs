using System.Collections.Generic;
using Quests;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestView questView;
    [SerializeField] private List<QuestSettings> availableQuests; 
    
    private Dictionary<ushort, Quest> _activeQuests; 
    private Dictionary<ushort, QuestSettings> _availableQuestLookup;
    private Dictionary<ushort, Quest> _completedQuests;
    
    private GameSaveManager _gameSaveManager;
    
    private GameData _gameData;
    
    private ushort _startQuestHash;
    
    public void Inject(DependencyContainer container)
    {
        _gameSaveManager = container.Resolve<GameSaveManager>();

        _availableQuestLookup = new Dictionary<ushort, QuestSettings>(availableQuests.Count);
        _activeQuests = new Dictionary<ushort, Quest>(availableQuests.Count);
        _completedQuests = new Dictionary<ushort, Quest>(availableQuests.Count);

        if (availableQuests.Count > 0)
        {
            _startQuestHash = availableQuests[0].UniqueID;
        }
        else
        {
            Debug.LogWarning("No available quests found!");
            return;
        }
        

        foreach (var questSettings in availableQuests)
        {
            var questHash = questSettings.UniqueID;
            if (!_availableQuestLookup.ContainsKey(questHash))
            {
                _availableQuestLookup.Add(questHash, questSettings);
            }
        }
        
        ActivateQuest(_startQuestHash);
    }
    
    public void RestoreQuestProgress(QuestProgress questProgress)
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

    private void Start()
    {
        questView.UpdateQuests();
    }

    public void ActivateQuest(ushort questHashCode)
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
    
    public void CompleteQuestStep(ushort questHashCode, ushort questStepHashCode)
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

    public Dictionary<ushort, Quest> GetQuests => _activeQuests;

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
    
}
