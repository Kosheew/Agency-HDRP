using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue/DialogueSettings")]
public class DialogueSettings : ScriptableObject
{
    [Title("Основні дані")]
    [BoxGroup("Основні дані")]
    [LabelText("Ім'я персонажа"), SerializeField]
    private string personName;
    
    [BoxGroup("Основні дані")]
    [LabelText("Унікальний ID"), SerializeField]
    private ushort uniqueID;
    
    [BoxGroup("Основні дані")]
    [LabelText("Текст діалогу"), SerializeField, TextArea(3, 5)]
    private string sentence;
    
    [BoxGroup("Основні дані")]
    [LabelText("Озвучка тексту"), SerializeField]
    private AudioClip voiceActingClip;
    
    [Space(10)]
    [BoxGroup("Основні дані"), ListDrawerSettings(Expanded = true, DraggableItems = true, ShowItemCount = true)] 
    [SerializeField] 
    private DialogueOptionSettings[] options;

    [Space(10)]
    [Title("Додаткові налаштування")]
    [BoxGroup("Додаткові налаштування")]
    [LabelText("Додати підказку?"), SerializeField]
    private bool addHint;
    
    [FormerlySerializedAs("hint")]
    [BoxGroup("Додаткові налаштування")]
    [ShowIf("@addHint"), LabelText("Підказка"), SerializeField]
    private CluesData clues;
    
    public string PersonName => personName;
    public ushort UniqueID => uniqueID;
    public string Sentence => sentence;
    public AudioClip VoiceActingClip => voiceActingClip;
    public DialogueOptionSettings[] Options => options;

    public void AddHint(CluesController cluesController)
    {
        if(addHint)
            cluesController.CollectClue(clues);
    }
    
}
