using UnityEngine;

[CreateAssetMenu(fileName = "Hint", menuName = "Dialogue/Hint")]
public class HintData : ScriptableObject
{
    [SerializeField] private ushort uniqueID;
    [SerializeField] private string hintName;
    [SerializeField, TextArea(1, 3)] private string description;
    
    public ushort UniqueID => uniqueID;
    public string HintName => hintName;
    public string Description => description;
}
