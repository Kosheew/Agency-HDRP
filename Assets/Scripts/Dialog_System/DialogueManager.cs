using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObjectPool;

public class DialogueManager : MonoBehaviour {
    
    [SerializeField] private DialogueSettings[] dialogueSettings;
    [SerializeField] private DialogueOptionSettings[] optionSettings;
    
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
        
        _currentNPCDialogueManager.SaveChosenOption(dialogue, null);
        
        _dialogueView.GenerateDialoguePanel(dialogue);
    }
    

    public void SelectOption(DialogueSettings dialogue, DialogueOptionSettings selectedOption)
    {
        if (selectedOption.NextDialogue != null)
        {
            _currentNPCDialogueManager.SaveChosenOption(dialogue, selectedOption);
            _currentNPCDialogueManager.SaveChosenOption(selectedOption.NextDialogue, null);
            
            _dialogueView.GenerateDialoguePanel(selectedOption.NextDialogue);
            Debug.Log(selectedOption.NextDialogue);
        }

    }
    
    public void ShowPassedDialogues(NPCDialogueManager npc)
    {
       _dialogueView.ShowPassedDialogues(npc);
    }
}