using UnityEditor;
using UnityEngine;
using System.Reflection;

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
        Texture2D customIconDialog = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Icons/Dialog.png");
        var customIconDialogOption = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Icons/DialogOptions.png");
        
        if (customIconDialog == null) return;

        foreach (var obj in Resources.FindObjectsOfTypeAll<DialogueSettings>())
        {
            EditorGUIUtility.SetIconForObject(obj, customIconDialog);
        }
        
        if(customIconDialogOption == null) return;

        foreach (var obj in Resources.FindObjectsOfTypeAll<DialogueOptionSettings>())
        {
            EditorGUIUtility.SetIconForObject(obj, customIconDialogOption);
        }
    }
    
}