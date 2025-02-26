using System.Collections.Generic;
using Dialog_System;
using UnityEngine;

public class NPCDialogueController : MonoBehaviour
{
    [SerializeField] private NPCFileData npcFileData;
    [SerializeField] private DialogueSettings currentDialogue;
    
    private DialogueController _dialogueController;
    private Dictionary<DialogueSettings, DialogueOptionSettings> _passedDialogueSettings;
    
    public void Inject(DependencyContainer container)
    {
        _dialogueController = container.Resolve<DialogueController>();
        _passedDialogueSettings = new Dictionary<DialogueSettings, DialogueOptionSettings>(10);
    }
    
    public void StartDialogue()
    {
        if(_dialogueController == null) return;
        
        _dialogueController.SetNpcDialogueManager(this);
        
        if (_passedDialogueSettings.Count > 0)
        {
            _dialogueController.ShowPassedDialogues(this);
        }
        else if (currentDialogue != null)
        {
            _dialogueController.StartDialogue(currentDialogue);
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

    /*public NPCDialogProgress GetDialogueProgress()
    {
        return new NPCDialogProgress(npcFileData.UniqueID, _passedDialogueSettings);
    }
    
    public void LoadDialogueProgress(NPCDialogProgress progress)
    {
       // if (progress == null || progress.PassedDialogues == null) return;
       // _passedDialogueSettings = new Dictionary<DialogueSettings, DialogueOptionSettings>(progress.PassedDialogues);
    }*/
}
