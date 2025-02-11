using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestStep
{
    public bool IsCompleted { get; private set; }

    public void CompleteQuest()
    {
        IsCompleted = true;
    }
    
}
