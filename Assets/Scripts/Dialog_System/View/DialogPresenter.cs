using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogPresenter
{
    private DialoguePanelView _panelView;
    private EvidenceManager _evidenceManager;
    private QuestManager _questManager;
    private DialogueManager _dialogueManager;
    
    public DialogPresenter(DialoguePanelView panelView, DependencyContainer container)
    {
        _panelView = panelView;
        _evidenceManager = container.Resolve<EvidenceManager>();
        _questManager = container.Resolve<QuestManager>();
        _dialogueManager = container.Resolve<DialogueManager>();
        _panelView.Inject(this);
    }
    
    public void SetDialogueOptions(DialogueSettings dialogue, Button[] buttons, TMP_Text[] texts)
    {
        int index = 0;
        foreach (var option in dialogue.Options)
        {
            var btn = buttons[index];
            btn.gameObject.SetActive(true);
            texts[index].SetText(option.IsAvailable(_evidenceManager) ? option.Sentence : "not available");
            
            btn.onClick.RemoveAllListeners();
            if (option.IsAvailable(_evidenceManager))
            {
                
                btn.onClick.AddListener(() =>
                {
                    _panelView.ShowSelectedOption(option.Sentence);
                    _panelView.SetActiveButtons(false);
                    
                    if (option.Quest != null)
                        option.AddQuest(_questManager);
                    
                    if (option.NextDialogue != null)
                    {
                        _dialogueManager.SelectOption(dialogue, option);
                    }
                });
            }
            index++;
        }
    }
}
