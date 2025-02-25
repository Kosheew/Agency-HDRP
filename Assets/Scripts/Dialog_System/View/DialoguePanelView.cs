using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;
using System.Threading;

public class DialoguePanelView : MonoBehaviour
{
    [SerializeField] private TMP_Text personNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] optionsButton;
    [SerializeField] private TMP_Text[] optionsText;

    [SerializeField] private TMP_Text selectedOptionText;
    [SerializeField] private TMP_Text youText;

    [SerializeField] private float typingSpeed = 0.05f;
    
    private DialogPresenter _dialogPresenter;
    private Coroutine _typingCoroutine;
    private DialogueSettings _currentDialogueSettings;
    
    private string _currentDialogue = ""; 
    
    public void Inject(DialogPresenter dialogPresenter)
    {
        _dialogPresenter = dialogPresenter;
    }
    
    public void SetDialogue(DialogueSettings dialogue)
    {
        personNameText.SetText(dialogue.PersonName);

        _currentDialogue = dialogue.Sentence;
        _currentDialogueSettings = dialogue;
        
        ResetTyping();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        StringBuilder sb = new StringBuilder();
        dialogueText.SetText(""); 

        foreach (char letter in sentence)
        {
            sb.Append(letter);
            dialogueText.SetText(sb.ToString());
            yield return new WaitForSeconds(typingSpeed);
        }
        
        _dialogPresenter.SetDialogueOptions(_currentDialogueSettings, optionsButton, optionsText);
    }

    public void StartTyping()
    {
        AudioManager.Instance.PlayDialogueAudio(_currentDialogueSettings.VoiceActingClip);
        _typingCoroutine = StartCoroutine(TypeSentence(_currentDialogueSettings.Sentence));
    }
    
    public void SkipTyping()
    {
        ResetTyping();
        dialogueText.SetText(_currentDialogue); 
        _dialogPresenter.SetDialogueOptions(_currentDialogueSettings, optionsButton, optionsText);
    }
    
    private void ResetTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }
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
    
        ResetTyping();
        
        personNameText.SetText(string.Empty);
        dialogueText.SetText(string.Empty);
    
        selectedOptionText?.SetText(string.Empty);
        selectedOptionText?.gameObject.SetActive(false);
        youText?.gameObject.SetActive(false);
        
        AudioManager.Instance.StopAudio();
    }
}