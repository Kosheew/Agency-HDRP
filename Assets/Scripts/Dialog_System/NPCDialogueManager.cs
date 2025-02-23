using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSettings currentDialogue;
    
    private DialogueManager _dialogueManager;
    private Dictionary<DialogueSettings, DialogueOptionSettings> _passedDialogueSettings;
    
    public void Inject(DependencyContainer container)
    {
        _passedDialogueSettings = new Dictionary<DialogueSettings, DialogueOptionSettings>(10);
        _dialogueManager = container.Resolve<DialogueManager>();
    }
    
    public void StartDialogue()
    {
        if(_dialogueManager == null) return;
        
        _dialogueManager.SetNpcDialogueManager(this);
        
        if (_passedDialogueSettings.Count > 0)
        {
            _dialogueManager.ShowPassedDialogues(this);
        }
        else if (currentDialogue != null)
        {
            _dialogueManager.StartDialogue(currentDialogue);
        }
    }

    public void SetDialogue(DialogueSettings dialogueSettings)
    {
        currentDialogue = dialogueSettings;
        _passedDialogueSettings.Clear();
    }
    
    public void SaveChosenOption(DialogueSettings dialogue, DialogueOptionSettings chosenOption)
    {
        if (_passedDialogueSettings.ContainsKey(dialogue))
        {
            _passedDialogueSettings[dialogue] = chosenOption;
            return;
        }
        
        _passedDialogueSettings.Add(dialogue, chosenOption);
    }
    
    public Dictionary<DialogueSettings, DialogueOptionSettings> GetPassedDialogues()
    {
        return _passedDialogueSettings;
    }
}
