using System;
using System.Collections.Generic;

namespace Dialog_System
{
    [Serializable]
    public class DialogProgressData
    {
        public List<ushort> CompletedDialogues;

        public DialogProgressData(HashSet<ushort> completedDialogues)
        {
            CompletedDialogues = new List<ushort>(completedDialogues);
        }
    }
}