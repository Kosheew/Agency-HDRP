using UnityEditor;
using UnityEngine;
using System.Reflection;
using Quests;

[InitializeOnLoad]
public static class ScriptableObjectIconSetter
{
    static ScriptableObjectIconSetter()
    {
        EditorApplication.delayCall += SetIcons;
    }

    [MenuItem("Tools/Set ScriptableObject Icons")]
    private static void ForceSetIcons()
    {
        SetIcons();
    }
    
    private static void SetIcons()
    {
        ApplyIcon<DialogueSettings>("Assets/Icons/Dialog.png");
        ApplyIcon<DialogueOptionSettings>("Assets/Icons/DialogOptions.png");
        ApplyIcon<NPCFileData>("Assets/Icons/NPCFile.png");
        ApplyIcon<GameEventData>("Assets/Icons/Event.png");
        ApplyIcon<HintData>("Assets/Icons/Hint.png");
        ApplyIcon<QuestSettings>("Assets/Icons/Quest.png");

        // Збереження змін
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void ApplyIcon<T>(string iconPath) where T : ScriptableObject
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
        if (texture == null) return;

        foreach (var obj in Resources.FindObjectsOfTypeAll<T>())
        {
            EditorGUIUtility.SetIconForObject(obj, texture);
            EditorUtility.SetDirty(obj);  // Позначаємо об'єкт як змінений
        }
    }
}