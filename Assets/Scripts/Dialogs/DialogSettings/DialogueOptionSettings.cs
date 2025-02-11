using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Option", menuName = "Dialogue/DialogueOptionSettings")]
public class DialogueOptionSettings: ScriptableObject
{
    [SerializeField] private ushort uniqueID;
    [SerializeField, TextArea(3, 5)] private string sentence;
    
    public ushort UniqueID => uniqueID;
    public string Sentence => sentence;
}
