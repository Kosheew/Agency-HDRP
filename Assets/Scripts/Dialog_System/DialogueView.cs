using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private TMP_Text personNameText;
    [SerializeField] private Image personPortrait;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] optionsButton;
    [SerializeField] private TMP_Text[] optionsText;

    [SerializeField] private TMP_Text selectedOptionText;
    [SerializeField] private TMP_Text youText;
    
    private EvidenceManager _evidenceManager;
    private QuestManager _questManager;

    public void Inject(DependencyContainer container)
    {
        _evidenceManager = container.Resolve<EvidenceManager>();
        _questManager = container.Resolve<QuestManager>();
    }
    
    public void SetDialogue(DialogueSettings dialogue)
    {
        personNameText.SetText(dialogue.PersonName);
        personPortrait.sprite = dialogue.PersonPortrait;
        dialogueText.SetText(dialogue.Sentence);

        SetActiveButtons(false);

        int index = 0;
        
        foreach (var dialogueOption in dialogue.Options)
        { 
            var btn = optionsButton[index];
            
            btn.gameObject.SetActive(true);
            
            if (dialogueOption.IsAvailable(_evidenceManager))
            {
                btn.onClick.RemoveAllListeners();
                
                if (dialogueOption.Quest != null)
                {
                    btn.onClick.AddListener(() =>
                    {
                        dialogueOption.AddQuest(_questManager);
                    });
                }

                btn.onClick.AddListener(() =>
                {
                    SelectedOption(dialogueOption.Sentence);
                    SetActiveButtons(false);
                });

                optionsText[index].SetText(dialogueOption.Sentence);
            }
            else
            {
                optionsText[index].SetText("not available");
            }
            
            index++;
        }
    }

    private void SelectedOption(string optionText)
    {
        selectedOptionText.gameObject.SetActive(true);
        youText.gameObject.SetActive(true);
        selectedOptionText.SetText(optionText);
    }

    private void SetActiveButtons(bool active)
    {
        foreach (var button in optionsButton)
        {
            button.gameObject.SetActive(active);
        }
    }
    
}