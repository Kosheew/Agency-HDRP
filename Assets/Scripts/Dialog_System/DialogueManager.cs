using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ObjectPool;

public class DialogueManager : MonoBehaviour
{
    
    [SerializeField] private GameObject dialogueViewPrefab;
    [SerializeField] private GameObject context;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private DialogueSettings[] dialogueSettings;
    [SerializeField] private DialogueOptionSettings[] optionSettings;
    
    private DialogueSettings _currentDialogue;
    private DialogueView _dialogueView;
    private DependencyContainer _dependencyContainer;
    private DialogPresenter _dialogPresenter;
    
    private bool _isDialogueOpen;
    
    private NPCDialogueManager _currentNPCDialogueManager;
    
    List<DialogueView> panels = new List<DialogueView>(20);
    
    private CustomPool<DialogueView> _dialogueViewPool;
    
    public void Inject(DependencyContainer container)
    {
        _dependencyContainer = container;
        
        DialogueView view = dialogueViewPrefab.GetComponent<DialogueView>();
        _dialogueViewPool = new CustomPool<DialogueView>(view, 20, context.transform);
    }

    public void SetNpcDialogueManager(NPCDialogueManager dialogueManager)
    {
        _currentNPCDialogueManager = dialogueManager;
    }
    
    public void StartDialogue(DialogueSettings dialogue)
    {
        if(_isDialogueOpen) return;
        
        _isDialogueOpen = true;
        _animator.SetTrigger("Show");
        
        GenerateDialogue(dialogue);
    }

    public void GenerateDialogue(DialogueSettings dialogue)
    {
        _currentDialogue = dialogue;
        _dialogueView = _dialogueViewPool.Get();
    
        _dialogueView.ResetView(); // Скидаємо стан перед налаштуванням нового діалогу

        new DialogPresenter(_dialogueView, _dependencyContainer);

        panels.Add(_dialogueView);
        _dialogueView.SetDialogue(_currentDialogue);
    }


    public void SelectOption(DialogueSettings dialogue, DialogueOptionSettings selectedOption)
    {
        if (selectedOption.NextDialogue != null)
        {
            _currentNPCDialogueManager.SaveChosenOption(dialogue.UniqueID, selectedOption.UniqueID);// Додаємо в історію
            GenerateDialogue(selectedOption.NextDialogue);
        }
        else
        {
            HidePanels();
        }
    }
    
    public void ShowPassedDialogues(NPCDialogueManager npc)
    {
        if(_isDialogueOpen) return;
        
        _isDialogueOpen = true;
        _animator.SetTrigger("Show");
        
        var passedDialogues = npc.GetPassedDialogues();
        foreach (var dialogue in passedDialogues)
        {
            var dialog = dialogueSettings.FirstOrDefault(d => d.UniqueID == dialogue.Item1);
            if(dialog == null) continue;
            GenerateDialogue(dialog);
            if (dialogue.Item2 != 0)
            {
                var option = optionSettings.FirstOrDefault(o => o.UniqueID == dialogue.Item2);
                if(option == null) continue;
                _dialogueView.ShowSelectedOption(option.Sentence);
                _dialogueView.SetActiveButtons(false);
            }
            
        }
    }

    
    public void HidePanels()
    {
        _animator.SetTrigger("Hide");
        _isDialogueOpen = false;

        _currentNPCDialogueManager.SaveChosenOption(_currentDialogue.UniqueID, 0);
        
        foreach (var obj in panels)
        {
            _dialogueViewPool.Release(obj);
        }
        panels.Clear();
        
    }
}