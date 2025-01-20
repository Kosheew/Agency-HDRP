using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialogs")]
public class DialogParams : ScriptableObject
{
    [SerializeField] private Sprite _personPhoto;
    [SerializeField] private string _personName;
    [SerializeField] private string _text;
    [SerializeField] private string[] _optionsName;
    [SerializeField] private DialogParams[] _options;
    [SerializeField] private AudioClip _audio;

    public Sprite PersonPhoto => _personPhoto;
    public string PersonName => _personName;
    public string Text => _text;
    public string[] OptionsName => _optionsName;
    public DialogParams[] Options => _options;
    public AudioClip Audio => _audio;
}
