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

        foreach (var button in optionsButton)
        {
            button.gameObject.SetActive(false);
        }

        int index = 0;
        
        foreach (var dialogueOption in dialogue.Options)
        { 
            optionsButton[index].gameObject.SetActive(true);
            
            if (dialogueOption.IsAvailable(_evidenceManager))
            {
                optionsButton[index].onClick.RemoveAllListeners();
                
                if (dialogueOption.Quest != null)
                {
                    optionsButton[index].onClick.AddListener(() =>
                    {
                        dialogueOption.AddQuest(_questManager);
                    });
                }
                
                optionsText[index].SetText(dialogueOption.Sentence);
            }
            else
            {
                optionsText[index].SetText("not available");
            }
            
            index++;
        }
    }
    
}