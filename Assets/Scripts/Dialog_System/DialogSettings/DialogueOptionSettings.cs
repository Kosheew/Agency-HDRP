using Quests;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Option", menuName = "ScriptableObjects/Dialogue/DialogueOptionSettings")]
public class DialogueOptionSettings: ScriptableObject
{
    [Title("Основні дані")]
    
    [BoxGroup("Основні дані")]
    [LabelText("Унікальний ID"), SerializeField]
    private ushort uniqueID;
    
    [BoxGroup("Основні дані")]
    [LabelText("Основний текст"), SerializeField, TextArea(3, 8)]
    private string sentence;
    
    [Space(10)]
    [BoxGroup("Основні дані")]
    [LabelText("Наступний діалог"), SerializeField] 
    private DialogueSettings nextDialogue;
    
    [Title("Додаткові дані")]
    
    [BoxGroup("Додаткові дані")]
    [LabelText("Необхідна зачіпка?"), SerializeField]
    private bool needHint;
    
    [ShowIf(nameof(needHint)), BoxGroup("Додаткові дані"), SerializeField]
    private HintData hint;

    [BoxGroup("Додаткові дані"), LabelText("Відкрити квест?"), SerializeField]
    private bool adderQuest;
    
    [ShowIf(nameof(adderQuest)), BoxGroup("Додаткові дані"), SerializeField]
    private QuestSettings quest;
    
    [BoxGroup("Додаткові дані"),LabelText("Викликати подію?"), SerializeField]
    private bool addAction;
    
    [ShowIf(nameof(addAction)), BoxGroup("Додаткові дані"), SerializeField]
    private GameEventData gameEvent;
    
    public ushort UniqueID => uniqueID;
    public string Sentence => sentence;
    public DialogueSettings NextDialogue => nextDialogue;
    public QuestSettings Quest => quest;
    public GameEventData GameEvent => gameEvent;
    
    public bool IsAvailable(EvidenceManager evidenceManager)
    {
        if (!needHint) return true;
        
        return evidenceManager.HasHint(hint);
    }

    public void AddQuest(QuestManager questManager)
    {
        questManager.ActivateQuest(quest.UniqueID);
    }
}