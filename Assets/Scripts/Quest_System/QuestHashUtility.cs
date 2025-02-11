using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class QuestHashUtility
{
    public static int GetQuestHash(string uniqueID)
    {
        return string.IsNullOrEmpty(uniqueID) ? 0 : uniqueID.GetHashCode();
    }
}
