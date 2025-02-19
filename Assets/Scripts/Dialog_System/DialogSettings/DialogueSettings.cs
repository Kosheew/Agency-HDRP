using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Burst.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueSettings")]
public class DialogueSettings : ScriptableObject
{
    [SerializeField] private Sprite personPortrait;
    [SerializeField] private string personName;
    [SerializeField] private ushort uniqueID;
    [SerializeField, TextArea(3,5)] private string sentence;
    [SerializeField] private DialogueOptionSettings[] options;

    [SerializeField] private bool addHint;
    [ShowIf("@addHint")]
    [SerializeField] private HintData hint;
    
    public Sprite PersonPortrait => personPortrait;
    public string PersonName => personName;
    public ushort UniqueID => uniqueID;
    public string Sentence => sentence;
    public DialogueOptionSettings[] Options => options;

    public void AddHint(EvidenceManager evidenceManager)
    {
        evidenceManager.CollectHint(hint);
    }
    
}
