using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ObjectPool;
using TMPro;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private TMP_Text personNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] optionsButton;
    [SerializeField] private TMP_Text[] optionsText;

    [SerializeField] private TMP_Text selectedOptionText;
    [SerializeField] private TMP_Text youText;

    private DialogPresenter _dialogPresenter;
    
    public void Inject(DialogPresenter dialogPresenter)
    {
        _dialogPresenter = dialogPresenter;
    }
    
    public void SetDialogue(DialogueSettings dialogue)
    {
        personNameText.SetText(dialogue.PersonName);
        dialogueText.SetText(dialogue.Sentence);

        bool hasOptions = dialogue.Options.Length > 0;
        SetActiveButtons(hasOptions);
        
        _dialogPresenter.SetDialogueOptions(dialogue, optionsButton, optionsText);
    }

    public void ShowSelectedOption(string optionText)
    {
        if (selectedOptionText != null)
        {
            selectedOptionText.gameObject.SetActive(true);
            youText.gameObject.SetActive(true);
            selectedOptionText.SetText(optionText);
        }
    }

    public void SetActiveButtons(bool active)
    {
        foreach (var button in optionsButton)
        {
            button.gameObject.SetActive(active);
        }
    }
    
    public void ResetView()
    {
        SetActiveButtons(false); // Увімкнути кнопки
        
        foreach (var optionText in optionsText)
        {
            optionText.SetText(string.Empty);
        }
    
        personNameText.SetText(string.Empty);
        dialogueText.SetText(string.Empty);
    
        selectedOptionText?.SetText(string.Empty);
        selectedOptionText?.gameObject.SetActive(false);
        youText?.gameObject.SetActive(false);
    }

}