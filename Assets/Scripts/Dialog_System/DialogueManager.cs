using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueView dialogueView;
    
    private DialogueSettings _currentDialogue;
    
    private EvidenceManager _evidenceManager;
    private QuestManager _questManager;

    public void Inject(DependencyContainer container)
    {
        _evidenceManager = container.Resolve<EvidenceManager>();
        _questManager = container.Resolve<QuestManager>();
    }

    public void StartDialogue(DialogueSettings dialogue)
    {
        _currentDialogue = dialogue;
        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        dialogueView.SetDialogue(_currentDialogue);
        
    }

    private void SelectOption(DialogueOptionSettings selectedOption)
    {
        if (selectedOption.NextDialogue != null)
        {
            _currentDialogue = selectedOption.NextDialogue;
            ShowCurrentDialogue();
        }
        else
        {
        
        }
    }
}