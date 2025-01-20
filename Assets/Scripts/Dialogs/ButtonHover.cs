using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _hoverTextColor; 
    [SerializeField] private AudioClip _hoverSound;
    
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
        if(_button.interactable)
            _buttonText.color = _hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_button.interactable)
            _buttonText.color = _normalTextColor;
    }
}
