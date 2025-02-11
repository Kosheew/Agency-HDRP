using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCFile", menuName = "NPC/NPCFile")]
public class NPCFileData : ScriptableObject
{
    [SerializeField] private Sprite spritePerson;
    [SerializeField] private string description;
    
    [SerializeField] private HintData[] hints;
    
    public Sprite SpritePerson => spritePerson;
    public string Description => description;
    public HintData[] Hints => hints;
    
    public List<HintData> GetKnownHints(EvidenceManager evidenceManager)
    {
        var knownHints = new List<HintData>();

        foreach (var hint in hints)
        {
            if (evidenceManager.HasHint(hint) && !evidenceManager.IsHintArchived(hint))
            {
                knownHints.Add(hint);
            }
        }

        return knownHints;
    }
}
