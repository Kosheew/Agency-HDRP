using System;
using System.Collections.Generic;
using Dialog_System;
using UnityEngine.Serialization;

[Serializable] 
public class GameData
{
    public NPCDialogProgressData npcDialogProgressData;
    public QuestProgressData questProgressData;
    [FormerlySerializedAs("evidenceProgressData")] public ClueProgressData clueProgressData;
    
    public GameData()
    {
        npcDialogProgressData = new NPCDialogProgressData(new List<NPCDialogProgress>(10));
        questProgressData = new QuestProgressData(new List<QuestProgress>(10));
        clueProgressData = new ClueProgressData(new HashSet<ushort>(10), new HashSet<ushort>(10));
    }
}