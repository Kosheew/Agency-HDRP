using UnityEngine;

[CreateAssetMenu(fileName = "Hint", menuName = "ScriptableObjects/NPC/Hint")]
public class CluesData : ScriptableObject
{
    [SerializeField] private ushort uniqueID;
    [SerializeField] private string hintName;
    [SerializeField, TextArea(1, 3)] private string description;
    
    public ushort UniqueID => uniqueID;
    public string HintName => hintName;
    public string Description => description;
}
