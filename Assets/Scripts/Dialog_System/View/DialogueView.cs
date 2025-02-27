using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private GameObject dialogueViewPrefab;
    [SerializeField] private GameObject context;
    [SerializeField] private int maxPanelCount = 20;
    
    
    private Animator _animator;
    private int _animatorIDShow;
    private int _animatorIDHide;
    
    private bool _isDialogueOpen;
    private CustomPool<DialoguePanelView> _dialogueViewPool;
    private Stack<DialoguePanelView> _activePanels;
    private DependencyContainer _dependencyContainer;
    
    private DialoguePanelView _currentPanel;
    
    public void Inject(DependencyContainer container)
    {
        _animator = GetComponent<Animator>();
        
        DialoguePanelView panelView = dialogueViewPrefab.GetComponent<DialoguePanelView>();
        _dialogueViewPool = new CustomPool<DialoguePanelView>(panelView, maxPanelCount, context.transform);
        _activePanels = new Stack<DialoguePanelView>(maxPanelCount);
        
        _dependencyContainer = container;
        
        InitAnimation();
    }

    private void InitAnimation()
    {
        _animatorIDHide = Animator.StringToHash("Hide");
        _animatorIDShow = Animator.StringToHash("Show");
    }

    public void SetDialoguePanel(DialogueSettings dialogue)
    { 
        _currentPanel = _dialogueViewPool.Get();
                
        _currentPanel.ResetView();
                
        new DialogPresenter(_currentPanel, _dependencyContainer);
        
        _activePanels.Push(_currentPanel);
        
        _currentPanel.SetDialogue(dialogue);
    }

    public void TypingText()
    {
        _currentPanel.StartTyping();
    }
    
    public bool IsDialogueOpen()
    {
        if(_isDialogueOpen) return true;
        
        _isDialogueOpen = true;
        _animator.SetTrigger(_animatorIDShow);
        
        return false;
    }

    public void CloseDialogueContext()
    {
        _animator.SetTrigger(_animatorIDHide);
        _isDialogueOpen = false;
        
        foreach (var obj in _activePanels)
        {
            obj.ResetView();
            if (_dialogueViewPool != null)
            {
                _dialogueViewPool.Release(obj);
            }
        }
        _activePanels.Clear();
    }
    
    public void ShowPassedDialogues(NPCDialogueController npc)
    {
        if (IsDialogueOpen()) return;
        
        var passedDialogues = npc.GetPassedDialogues();
        
        foreach (var dialogue in passedDialogues)
        {
            var dialog = dialogue.Key;
            
            if(dialog == null) continue;
            
            SetDialoguePanel(dialog);
            _currentPanel.SkipTyping();
            
            if (dialogue.Value != null)
            {
                var option = dialogue.Value;
                
                if(option == null) continue;
                
                _currentPanel.ShowSelectedOption(option.Sentence);
                _currentPanel.SetActiveButtons(false);
            }
        }
    }
}
