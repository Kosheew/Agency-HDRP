using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scroll;   
    
    [SerializeField] private float _panelHeight = 550f;
    [SerializeField] private float _panelWidth = 500f;
    
    [SerializeField] private Color _pressedColor;
    
    [SerializeField] private AudioSource _dialogsAudioSource;
    [SerializeField] private AudioSource _buttonAudioSource;

    private Image _personPhoto;
    private Text _personName;
    private TextMeshProUGUI _text;
    private List<Button> _answers;
    
    public void GenerateDialog(DialogParams dialog)
    {
        if (dialog.OptionsName.Length != dialog.Options.Length)
            throw new InvalidOperationException();
        
        if(dialog.Audio != null)
            PlayAudio(dialog.Audio);
        
        GameObject panel = Instantiate(_dialogPanel, _content);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(_panelHeight, _panelWidth);

        _personPhoto = panel.transform.Find("PersonPhoto").GetComponent<Image>();
        _personName = panel.GetComponentInChildren<Text>();
        _text = panel.GetComponentInChildren<TextMeshProUGUI>();
        _answers = panel.GetComponentsInChildren<Button>().ToList();

        _personPhoto.sprite = dialog.PersonPhoto;
        _personName.text = dialog.PersonName;
        _text.text = dialog.Text;

        for (int i = dialog.Options.Length; i < _answers.Count;)
        {
            Destroy(_answers[i].gameObject);
            _answers.RemoveAt(i);
        }

        for (int i = 0; i < dialog.Options.Length; i++)
        {
            int index = i;

            _answers[index].GetComponentInChildren<Text>().text = dialog.OptionsName[index];
            _answers[index].GetComponent<ButtonInteraction>().AddAudioSource(_buttonAudioSource);

            _answers[index].onClick.AddListener(() =>
            {
                ChangeTextColor(_answers[index]);
                DisableButtons(_answers);
                GenerateDialog(dialog.Options[index]);
            });
        }
    }

    private void DisableButtons(List<Button> buttons)
    {
        foreach (Button button in buttons)
            button.interactable = false;
    }

    private void ChangeTextColor(Button button) =>
        button.GetComponentInChildren<Text>().color = _pressedColor;

    private void PlayAudio(AudioClip audioClip)
    {
        _dialogsAudioSource.Stop();
        _dialogsAudioSource.PlayOneShot(audioClip);
    }
}
