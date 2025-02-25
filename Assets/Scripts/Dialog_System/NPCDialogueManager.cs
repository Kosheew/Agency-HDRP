using System.Collections.Generic;
using Dialog_System;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] private NPCFileData npcFileData;
    [SerializeField] private DialogueSettings currentDialogue;
    
    private DialogueManager _dialogueManager;
    private DialogProgressManager _dialogProgressManager;
    private Dictionary<DialogueSettings, DialogueOptionSettings> _passedDialogueSettings;
    
    public void Inject(DependencyContainer container)
    {
        _dialogueManager = container.Resolve<DialogueManager>();
        _dialogProgressManager = container.Resolve<DialogProgressManager>();

        _passedDialogueSettings = new Dictionary<DialogueSettings, DialogueOptionSettings>(10);
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
    
    public void AddChosenOption(DialogueSettings dialogue, DialogueOptionSettings chosenOption)
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

    public NPCDialogProgress GetDialogueProgress()
    {
        return new NPCDialogProgress(npcFileData.UniqueID, _passedDialogueSettings);
    }
}
