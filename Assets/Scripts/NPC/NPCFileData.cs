using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCFile", menuName = "ScriptableObjects/NPC/NPCFile")]
public class NPCFileData : ScriptableObject
{
    [SerializeField] private ushort uniqueID;
    [SerializeField] private Sprite spritePerson;
    [SerializeField] private string description;
    
    [SerializeField] private CluesData[] hints;
    
    public Sprite SpritePerson => spritePerson;
    public string Description => description;
    public CluesData[] Hints => hints;
    public ushort UniqueID => uniqueID;
    
    public List<CluesData> GetKnownHints(CluesController cluesController)
    {
        var knownHints = new List<CluesData>();

        foreach (var hint in hints)
        {
            if (cluesController.HasClue(hint) && !cluesController.IsClueArchived(hint))
            {
                knownHints.Add(hint);
            }
        }

        return knownHints;
    }
}
