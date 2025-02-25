using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager: MonoBehaviour
{
    private HashSet<ushort> _collectedHints;
    private HashSet<ushort> _archivedHints;
    private GameSaveManager _saveSystem;

    public void Inject(DependencyContainer container)
    {
        _saveSystem = container.Resolve<GameSaveManager>();

    
    //    var data = _saveSystem.gameData.evidenceProgressData;
            
           
        _collectedHints = new HashSet<ushort>(10);
        _archivedHints = new HashSet<ushort>(10);
        
    }

    public void CollectHint(HintData hint)
    {
        if (_collectedHints.Contains(hint.UniqueID))
        {
            Debug.LogWarning($"Hint '{hint.HintName}' already collected!");
            return;
        }

        _collectedHints.Add(hint.UniqueID);
        Debug.Log($"Collected new hint: {hint.HintName}");
    }

    public bool HasHint(HintData hint) => _collectedHints.Contains(hint.UniqueID);

    public void ArchiveHint(HintData hint)
    {
        if (_collectedHints.Contains(hint.UniqueID) && !_archivedHints.Contains(hint.UniqueID))
        {
            _archivedHints.Add(hint.UniqueID);
            Debug.Log($"Hint '{hint.HintName}' archived!");
        }
    }

    public bool IsHintArchived(HintData hint) => _archivedHints.Contains(hint.UniqueID);

    public List<HintData> GetVisibleHints(List<HintData> allHints)
    {
        var visibleHints = new List<HintData>();

        foreach (var hint in allHints)
        {
            if (_collectedHints.Contains(hint.UniqueID) && !_archivedHints.Contains(hint.UniqueID))
            {
                visibleHints.Add(hint);
            }
        }

        return visibleHints;
    }
    
}
