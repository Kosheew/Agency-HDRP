using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scroll;   
    
    [SerializeField] private float _panelHeight = 550f;
    [SerializeField] private float _panelWidth = 500f;
    
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private DialogueParams _firstPhrase;  
    [SerializeField] private int _maxPhrasesAmount;
    [SerializeField] private float _timeAfterLastPhrase;

    private Image _personPhoto;
    private Text _personName;
    private TextMeshProUGUI _text;
    private List<Button> _answers;
    
    private int _phrasesAmount;
    private bool _isFirstTimePlayed;

    private bool _panelActive = false;
    
    private List<GameObject> _activePanels;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _clickSound;
    private void Awake()
    {
        _activePanels = new List<GameObject>(15);
    }

    public void PlayFirstPhrase()
    {
        _scroll.gameObject.SetActive(true);
        GenerateDialogue(_firstPhrase);
        _isFirstTimePlayed = true;
    }
    
    public void GenerateDialogue(DialogueParams dialogue)
    {
        if (dialogue.OptionsName.Length != dialogue.Options.Length)
            throw new InvalidOperationException();
        
        if(dialogue.Audio != null)
            PlayAudio(dialogue.Audio);
        
        GameObject panel = Instantiate(_dialogPanel, _content);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(_panelHeight, _panelWidth);

        _activePanels.Add(panel);
        
        _personPhoto = panel.transform.Find("PersonPhoto").GetComponent<Image>();
        _personName = panel.GetComponentInChildren<Text>();
        _text = panel.GetComponentInChildren<TextMeshProUGUI>();
        _answers = panel.GetComponentsInChildren<Button>().ToList();

        _personPhoto.sprite = dialogue.PersonPhoto;
        _personName.text = dialogue.PersonName;
        _text.text = dialogue.Text;

        for (int i = dialogue.Options.Length; i < _answers.Count;)
        {
            _answers[i].gameObject.SetActive(false);
            _answers.RemoveAt(i);
        }
        
        for (int i = 0; i < dialogue.Options.Length; i++)
        {
            int index = i;

            _answers[index].GetComponentInChildren<Text>().text = dialogue.OptionsName[index];

            _answers[index].onClick.AddListener(() =>
            {
                if(_clickSound != null)
                    _audioSource.PlayOneShot(_clickSound);
                
                DisableButtons(_answers);
                GenerateDialogue(dialogue.Options[index]);
            });
        }
    }

    public void ShowPanel(DialogueParams dialogue)
    {
        if (_panelActive) return;
        
        PanelDestroyed();
        _animator.SetTrigger("Show");
        GenerateDialogue(dialogue);
        
        _panelActive = true;   
    }
    
    public void HidePanels()
    {
        if(!_panelActive) return;
        
        _maxPhrasesAmount = 0;
        _isFirstTimePlayed = false;
        PanelDestroyed();
        
        _animator.SetTrigger("Hide");
        _panelActive = false;
    }

    private void PanelDestroyed()
    {
        foreach (var panel in _activePanels)
        {
            Destroy(panel);
        }
        _activePanels.Clear();
    }
    
    #region  Helpers

        private void DisableButtons(List<Button> buttons)
        {
              foreach (Button button in buttons)
                button.interactable = false;
        }

        private void PlayAudio(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }

    #endregion
}
