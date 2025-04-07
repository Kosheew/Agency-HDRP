using System.Collections.Generic;
using Quests;
using Unity.VisualScripting;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] private QuestView questView;
    [SerializeField] private List<QuestSettings> availableQuests; 
    
    private Dictionary<ushort, Quest> _activeQuests; 
    private Dictionary<ushort, QuestSettings> _availableQuestLookup;
    private Dictionary<ushort, Quest> _completedQuests;
    
    private ushort _startQuestHash;
    
    public void Inject(DependencyContainer container)
    {
        _availableQuestLookup = new Dictionary<ushort, QuestSettings>(availableQuests.Count);
        _activeQuests = new Dictionary<ushort, Quest>(availableQuests.Count);
        _completedQuests = new Dictionary<ushort, Quest>(availableQuests.Count);

        InitializeQuests();
    }
    
    private void InitializeQuests()
    {
        if (availableQuests.Count == 0)
        {
            Debug.LogWarning("No available quests found!");
            return;
        }

        foreach (var questSettings in availableQuests)
        {
            _availableQuestLookup.TryAdd(questSettings.UniqueID, questSettings);
        }

        _startQuestHash = availableQuests[0].UniqueID;
        ActivateQuest(_startQuestHash);
    }
    
    public void RestoreQuestProgress(QuestProgress questProgress)
    {
        if (!_availableQuestLookup.TryGetValue(questProgress.QuestHash, out var questSettings))
        {
            Debug.LogWarning($"Quest with hash {questProgress.QuestHash} not found in available quests.");
            return;
        }

        var quest = new Quest(questSettings)
        {
            StepsCompleted = questProgress.GetStepsCompletedDictionary()
        };

        _activeQuests[questProgress.QuestHash] = quest;
        
        if (!quest.IsQuestCompleted())
        {
            questView.SetQuest(quest);
        }
    }
    
    public void ActivateQuest(ushort questHashCode)
    {
        if (!_availableQuestLookup.TryGetValue(questHashCode, out var questSettings))
        {
            Debug.LogWarning($"Quest with hash {questHashCode} not found in available quests!");
            return;
        }

        if (_activeQuests.ContainsKey(questHashCode))
        {
            Debug.LogWarning($"Quest {questSettings.QuestName} is already active!");
            return;
        }

        var newQuest = new Quest(questSettings);
                
        _activeQuests.Add(questHashCode, newQuest);
        questView.SetQuest(newQuest);
    }
    
    public void CompleteQuestStep(ushort questHashCode, ushort questStepHashCode)
    {
        if (!_activeQuests.TryGetValue(questHashCode, out var quest))
        {
            Debug.LogWarning($"Quest with hash {questHashCode} not found!");
            return;
        }

        quest.CompleteStep(questStepHashCode);
        questView.SetQuest(quest);

        if (!quest.IsQuestCompleted()) return;
            
        _activeQuests.Remove(questHashCode);
        _completedQuests.Add(questHashCode, quest);
        Debug.Log($"Quest {quest.GetQuestSO().QuestName} completed!");
                
        questView.UpdateQuests();
    }
    
    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(_activeQuests.Values);
    }

    public List<Quest> GetCompletedQuests()
    {
        return new List<Quest>(_completedQuests.Values);
    }
    
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
