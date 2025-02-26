using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestView : MonoBehaviour
{
    [Header("Quests Button")]
    [SerializeField] private Button nextQuestButton;
    [SerializeField] private Button previousQuestButton;
    
    [Header("Main Quest Text")]
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questDescription;
    
    [Header("Step Quest Text")]
    [SerializeField] private TMP_Text[] questStepTitle;
    [SerializeField] private TMP_Text[] questStepDescription;
    
    private QuestPresenter _questPresenter;
    
    public void Inject(DependencyContainer container)
    {
        _questPresenter = new QuestPresenter(container);

        nextQuestButton.onClick.AddListener(_questPresenter.NextQuest);
        previousQuestButton.onClick.AddListener(_questPresenter.PreviousQuest);

        UpdateQuests();
    }
    
    public void SetQuest(Quest quest)
    {
        var questSettings = quest.QuestSettings;        
        
        for (int i = 0; i < questStepTitle.Length; i++)
        {
            questStepTitle[i].gameObject.SetActive(false);
            questStepDescription[i].gameObject.SetActive(false);
        }
        
        questTitle.SetText(questSettings.QuestName);
        questDescription.SetText(questSettings.QuestDescription);

        var questSteps = questSettings.GetStepsDescriptions;
        
        for (int i = 0; i < questSettings.QuestStepsCount; i++)
        {
            var stepId = questSteps[i].UniqueID;
            bool isCompleted = quest.IsStepCompleted(stepId);
            
            Debug.Log(isCompleted);
            
            if (isCompleted)
            {
                 questStepTitle[i].fontStyle = FontStyles.Strikethrough;
                 questStepDescription[i].fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                questStepTitle[i].fontStyle = FontStyles.Normal;
                questStepDescription[i].fontStyle = FontStyles.Normal;
            }
            
            var questTitle = questStepTitle[i];
            var questDescription = questStepDescription[i];
            
            questTitle.SetText($"{i + 1}. {questSteps[i].QuestName}");
            questDescription.SetText(questSteps[i].Description);
            
            questTitle.gameObject.SetActive(true);
            questDescription.gameObject.SetActive(true);
        }
    }
    
    public void UpdateQuests()
    {
        _questPresenter.RefreshQuests();
    }

    public void Clear()
    {
        questTitle.SetText("Немає активних квестів");
        questDescription.SetText("");
    
        foreach (var title in questStepTitle)
            title.gameObject.SetActive(false);
    
        foreach (var description in questStepDescription)
            description.gameObject.SetActive(false);
    }

}
