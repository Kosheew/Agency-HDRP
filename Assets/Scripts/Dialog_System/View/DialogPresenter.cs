using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogPresenter
{
    private DialoguePanelView _panelView;
    private CluesController _cluesController;
    private QuestController _questController;
    private DialogueController _dialogueController;
    
    public DialogPresenter(DialoguePanelView panelView, DependencyContainer container)
    {
        _panelView = panelView;
        _cluesController = container.Resolve<CluesController>();
        _questController = container.Resolve<QuestController>();
        _dialogueController = container.Resolve<DialogueController>();
        _panelView.Inject(this);
    }
    
    public void SetDialogueOptions(DialogueSettings dialogue, Button[] buttons, TMP_Text[] texts)
    {
        int index = 0;
        foreach (var option in dialogue.Options)
        {
            var btn = buttons[index];
            btn.gameObject.SetActive(true);
            
            texts[index].SetText(option.IsAvailable(_cluesController) ? option.Sentence : "not available");
            
            btn.onClick.RemoveAllListeners();
            if (option.IsAvailable(_cluesController))
            {
                btn.onClick.AddListener(() =>
                {
                    _panelView.ShowSelectedOption(option.Sentence);
                    _panelView.SetActiveButtons(false);
                    
                    if (option.Quest != null)
                        option.AddQuest(_questController);
                    
                    if(option.GameEvent != null)
                        option.GameEvent.Raise();
                    
                    _dialogueController.SelectOption(dialogue, option);
                    
                });
            }
            index++;
        }
        dialogue.AddHint(_cluesController);
        
    }
}
