using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager: MonoBehaviour
{
    private HashSet<ushort> _collectedHints;
    private HashSet<ushort> _archivedHints;
    private BinarySaveSystem _saveSystem;

    public void Inject(DependencyContainer container)
    {
        _saveSystem = container.Resolve<BinarySaveSystem>();

        // Завантажуємо збережені докази
        if (_saveSystem.CheckFileExists())
        {
            var data = _saveSystem.Load<EvidenceProgressData>();
            _collectedHints = new HashSet<ushort>(data.CollectedHintIDs);
            _archivedHints = new HashSet<ushort>(data.ArchivedHintIDs);
        }
        else
        {
            _collectedHints = new HashSet<ushort>();
            _archivedHints = new HashSet<ushort>();
        }
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
    
    public void SaveProgress()
    {
        var progressData = new EvidenceProgressData(_collectedHints, _archivedHints);
        _saveSystem.Save(progressData);
        Debug.Log("Evidence progress saved!");
    }
}
