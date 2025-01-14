#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using SimpleDialogue;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(DialogueSystemStaticOptions))]
    public class DialogueSystemStaticOptionsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(5f);
            GUILayout.BeginVertical();
            GUILayout.Space(3f);
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Imported Object"), GUI.skin.label);
            GUILayout.FlexibleSpace();
            GUILayout.Space(3f);
            GUILayout.EndVertical();
        }
    }
}
#endif