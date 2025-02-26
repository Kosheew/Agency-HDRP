using UnityEngine;

public class DialogueController : MonoBehaviour 
{
    private NPCDialogueController _currentNpcDialogueController;
    private DialogueView _dialogueView;
    
    public void Inject(DependencyContainer container)
    {
        _dialogueView = container.Resolve<DialogueView>();
    }

    public void SetNpcDialogueManager(NPCDialogueController dialogueController)
    {
        _currentNpcDialogueController = dialogueController;
    }
    
    public void StartDialogue(DialogueSettings dialogue)
    {
        if (_dialogueView.IsDialogueOpen()  || dialogue == null || _currentNpcDialogueController == null) return;
        
        _currentNpcDialogueController.AddChosenOption(dialogue, null);
        
        ShowDialogue(dialogue);
    }

    public void SelectOption(DialogueSettings dialogue, DialogueOptionSettings selectedOption)
    {
        if (dialogue == null || selectedOption == null || _currentNpcDialogueController == null) return;
        
        _currentNpcDialogueController.AddChosenOption(dialogue, selectedOption);
        
        if (selectedOption.NextDialogue != null)
        {
            _currentNpcDialogueController.AddChosenOption(selectedOption.NextDialogue, null);
            
            _dialogueView.SetDialoguePanel(selectedOption.NextDialogue);
            
            _dialogueView.TypingText();
            
            Debug.Log(selectedOption.NextDialogue);
        }

    }
    
    public void ShowPassedDialogues(NPCDialogueController npc)
    {
       _dialogueView?.ShowPassedDialogues(npc);
    }
    
    private void ShowDialogue(DialogueSettings dialogue)
    {
        _dialogueView.SetDialoguePanel(dialogue);
        _dialogueView.TypingText();
    }
}