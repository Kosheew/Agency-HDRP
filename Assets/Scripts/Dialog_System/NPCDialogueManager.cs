using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueSettings dialogue;
    
    private DialogueManager _dialogueManager;
    private List<(ushort, ushort)> _passedDialogueSettings;
    
    public void Inject(DependencyContainer container)
    {
        _passedDialogueSettings = new List<(ushort, ushort)>(10);
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
        else if (_dialogueManager != null && dialogue != null)
        {
            _dialogueManager.StartDialogue(dialogue);
        }
    }

    public void SaveChosenOption(ushort dialogue, ushort chosenOption)
    {
        /*if (_passedDialogueSettings.ContainsKey(dialogue))
        {
            _passedDialogueSettings[dialogue] = chosenOption;
        }*/
        if (!_passedDialogueSettings.Contains((dialogue, chosenOption)))
        {
            _passedDialogueSettings.Add((dialogue, chosenOption));
        }
    }
    
    public List<(ushort, ushort)> GetPassedDialogues()
    {
        return _passedDialogueSettings;
    }
}
