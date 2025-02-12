using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSettings dialogue;
    
    private DialogueManager _dialogueManager;

    public void Inject(DialogueManager dialogueManager)
    {
        _dialogueManager = dialogueManager;
    }

    public void StartDialogue()
    {
        if (_dialogueManager != null && dialogue != null)
        {
            _dialogueManager.StartDialogue(dialogue);
        }
    }
}
