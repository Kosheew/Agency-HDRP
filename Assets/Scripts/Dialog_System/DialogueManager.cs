using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObjectPool;

public class DialogueManager : MonoBehaviour 
{
    
    private NPCDialogueManager _currentNPCDialogueManager;
    private DialogueView _dialogueView;
    
    public void Inject(DependencyContainer container)
    {
        _dialogueView = container.Resolve<DialogueView>();
    }

    public void SetNpcDialogueManager(NPCDialogueManager dialogueManager)
    {
        _currentNPCDialogueManager = dialogueManager;
    }
    
    public void StartDialogue(DialogueSettings dialogue)
    {
        if (_dialogueView.IsDialogueOpen()) return;
        
        _currentNPCDialogueManager.AddChosenOption(dialogue, null);
        
        _dialogueView.SetDialoguePanel(dialogue);
        _dialogueView.TypingText();
    }

    public void SelectOption(DialogueSettings dialogue, DialogueOptionSettings selectedOption)
    {
        _currentNPCDialogueManager.AddChosenOption(dialogue, selectedOption);
        
        if (selectedOption.NextDialogue != null)
        {
            _currentNPCDialogueManager.AddChosenOption(selectedOption.NextDialogue, null);
            
            _dialogueView.SetDialoguePanel(selectedOption.NextDialogue);
            
            _dialogueView.TypingText();
            Debug.Log(selectedOption.NextDialogue);
        }

    }
    
    public void ShowPassedDialogues(NPCDialogueManager npc)
    {
       _dialogueView.ShowPassedDialogues(npc);
    }
}