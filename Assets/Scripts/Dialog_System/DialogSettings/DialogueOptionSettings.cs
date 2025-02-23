using Quests;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Option", menuName = "ScriptableObjects/Dialogue/DialogueOptionSettings")]
public class DialogueOptionSettings: ScriptableObject
{
    [SerializeField] private ushort uniqueID;
    [SerializeField, TextArea(3, 5)] private string sentence;
    [SerializeField] private DialogueSettings nextDialogue;
    
    [SerializeField] private bool needHint;
    [ShowIf("@needHint")]
    [SerializeField] private HintData hint;

    [SerializeField] private bool adderQuest;
    [ShowIf("@adderQuest")]
    [SerializeField] private QuestSettings quest;
    
    public ushort UniqueID => uniqueID;
    public string Sentence => sentence;
    public DialogueSettings NextDialogue => nextDialogue;
    public QuestSettings Quest => quest;
    
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