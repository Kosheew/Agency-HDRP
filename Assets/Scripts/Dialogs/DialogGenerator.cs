using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private RectTransform _parentObject;
    
    [SerializeField] private float _spaceBetween = 10f;
    [SerializeField] private float _panelHeight = 500f;
    
    [SerializeField] private Color _pressedColor;
    
    private AudioSource _audioSource;

    private Image _personPhoto;
    private Text _personName;
    private TextMeshProUGUI _text;
    private List<Button> _answers;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void GenerateDialog(DialogParams dialog)
    {
        if (dialog.OptionsName.Length != dialog.Options.Length)
            throw new InvalidOperationException();
        
        if(dialog.Audio != null)
            _audioSource.PlayOneShot(dialog.Audio);
        
        GameObject panel = Instantiate(_dialogPanel, _parentObject);

        float panelHeight = panel.GetComponent<RectTransform>().rect.height;
        int panelCount = _parentObject.childCount;
        Vector3 newPosition = new Vector3(0, (-panelHeight + -_spaceBetween) * (panelCount - 1), 0);
        panel.GetComponent<RectTransform>().anchoredPosition = newPosition;

        panel.transform.localScale = Vector3.one;

        //float newHeight = (_parentObject.childCount * _panelHeight) +
        //                  ((_parentObject.childCount - 1) * _spaceBetween);
        //_parentObject.sizeDelta = new Vector2(_parentObject.sizeDelta.x, newHeight);

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
        _audioSource.Stop();
        _audioSource.PlayOneShot(audioClip);
    }
}
