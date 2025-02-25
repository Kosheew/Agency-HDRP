using System.Collections.Generic;
using Dialog_System;

public class NPCDialogSaveSystem : ISaveable
{
    private NPCDialogueManager[] _npcDialogues;

    public NPCDialogSaveSystem(NPCDialogueManager[] npcDialogues)
    {
        _npcDialogues = npcDialogues;
    }

    public void SaveTo(GameData gameData)
    {
        var npcProgressData = new List<NPCDialogProgress>();

        foreach (var npc in _npcDialogues)
        {
            var progress = npc.GetComponent<NPCDialogProgress>();
            if (progress != null) npcProgressData.Add(progress);
        }

        gameData.npcDialogProgressData = new NPCDialogProgressData(npcProgressData);
    }

    public void LoadFrom(GameData gameData)
    {
        if (gameData.npcDialogProgressData == null) return;

        foreach (var npc in _npcDialogues)
        {
            var progress = npc.GetComponent<NPCDialogProgress>();
            
            if (progress != null)
            {
              //  progress.LoadProgress(gameData.npcDialogProgressData);
            }
        }
    }
}