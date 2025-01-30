using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _hoverColor; 
    [SerializeField] private Color _pressedColor;
    [SerializeField] private AudioClip _hoverSound;
    
    private AudioSource _audioSource;   
    private Color _normalTextColor;
    private Text _buttonText;
    private Button _button;

    private void Start()
    {
        _buttonText = GetComponentInChildren<Text>();
        _button = GetComponent<Button>();
        _normalTextColor = _buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _buttonText.color = _hoverColor;
            _audioSource.PlayOneShot(_hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_button.interactable)
            _buttonText.color = _normalTextColor;
    }

    public void AddAudioSource(AudioSource audioSource) =>
        _audioSource = audioSource;
    
    public void ChangeTextColor(Button button) =>
        button.GetComponentInChildren<Text>().color = _pressedColor;
}
