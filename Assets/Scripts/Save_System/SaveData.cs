using System;
using System.Collections.Generic;
using Dialog_System;

[Serializable] 
public class GameData
{
    public NPCDialogProgressData npcDialogProgressData;
    public QuestProgressData questProgressData;
    public EvidenceProgressData evidenceProgressData;
    
    public GameData()
    {
        npcDialogProgressData = new NPCDialogProgressData(new List<NPCDialogProgress>(10));
        questProgressData = new QuestProgressData(new List<QuestProgress>(10));
        evidenceProgressData = new EvidenceProgressData(new HashSet<ushort>(10), new HashSet<ushort>(10));
    }
}