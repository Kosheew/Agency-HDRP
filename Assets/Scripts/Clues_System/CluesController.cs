using System.Collections.Generic;
using UnityEngine;

public class CluesController: MonoBehaviour
{
    private HashSet<ushort> _collectedHints;
    private HashSet<ushort> _archivedHints;
    private GameSaveManager _saveSystem;

    public void Inject(DependencyContainer container)
    {
        _saveSystem = container.Resolve<GameSaveManager>();
        
        _collectedHints = new HashSet<ushort>(10);
        _archivedHints = new HashSet<ushort>(10);
        
    }

    public void CollectClue(CluesData clues)
    {
        if (_collectedHints.Contains(clues.UniqueID))
        {
            Debug.LogWarning($"Hint '{clues.HintName}' already collected!");
            return;
        }

        _collectedHints.Add(clues.UniqueID);
        Debug.Log($"Collected new clues: {clues.HintName}");
    }

    public bool HasClue(CluesData clues) => _collectedHints.Contains(clues.UniqueID);

    public void ArchiveClue(CluesData clues)
    {
        if (_collectedHints.Contains(clues.UniqueID) && !_archivedHints.Contains(clues.UniqueID))
        {
            _archivedHints.Add(clues.UniqueID);
            Debug.Log($"Hint '{clues.HintName}' archived!");
        }
    }

    public bool IsClueArchived(CluesData clues) => _archivedHints.Contains(clues.UniqueID);

    public List<CluesData> GetVisibleClue(List<CluesData> allHints)
    {
        var visibleHints = new List<CluesData>();

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
