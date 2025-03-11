using System;
using System.Collections.Generic;

[Serializable]
public class ClueProgressData
{
    public List<ushort> CollectedHintIDs;
    public List<ushort> ArchivedHintIDs;

    public ClueProgressData(HashSet<ushort> collectedHints, HashSet<ushort> archivedHints)
    {
        CollectedHintIDs = new List<ushort>(collectedHints);
        ArchivedHintIDs = new List<ushort>(archivedHints);
    }
}