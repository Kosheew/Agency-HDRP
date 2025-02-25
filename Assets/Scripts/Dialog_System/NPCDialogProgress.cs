using System;
using System.Collections.Generic;

namespace Dialog_System
{
    [Serializable]
    public class NPCDialogProgress
    {
        public ushort NpcId;
        public List<ushort> CompletedDialogues;
        public List<ushort> ChooseDialoguesOptions;
        
        public NPCDialogProgress(ushort npcId, Dictionary<DialogueSettings, DialogueOptionSettings> passedDialogues)
        {
            CompletedDialogues = new List<ushort>();
            ChooseDialoguesOptions = new List<ushort>();
            
            foreach (var entry in passedDialogues)
            {
                CompletedDialogues.Add(entry.Key.UniqueID);
                
                if(entry.Value != null)
                    ChooseDialoguesOptions.Add(entry.Value.UniqueID);
                else
                    ChooseDialoguesOptions.Add(0);
            }
        }

        public Dictionary<ushort, ushort> GetDialogueProgress()
        {
            var progress = new Dictionary<ushort, ushort>();

            for (int i = 0; i < CompletedDialogues.Count; i++)
            {
                progress[CompletedDialogues[i]] = ChooseDialoguesOptions[i];
            }
            return progress;
        }
    }

    [Serializable]
    public class NPCDialogProgressData
    {
        public List<NPCDialogProgress> dialoguesProgress;

        public NPCDialogProgressData(List<NPCDialogProgress> dialoguesProgress)
        {
            this.dialoguesProgress = dialoguesProgress;
        }
    }
}