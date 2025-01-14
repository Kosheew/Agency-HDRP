#if UNITY_EDITOR
using SimpleDialogue;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogueEditor
{
    [InitializeOnLoad]
    public static class InspectorFocusChecker
    {
        static InspectorFocusChecker()
        {
            EditorApplication.delayCall += CheckInspectorFocus;
            CheckInspectorFocus();
        }

        private static void CheckInspectorFocus()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length > 0)
            {
                foreach (GameObject obj in selectedObjects)
                {
                    if (
                        obj.TryGetComponent<DialoguePlayer>(out DialoguePlayer player) ||
                        obj.TryGetComponent<NPCDialoguePlayer>(out NPCDialoguePlayer npcplayer) ||
                        obj.TryGetComponent<DialogueSystemBrain>(out DialogueSystemBrain brain)
                        )
                    {
                        Selection.activeObject = null;
                        break;
                    }
                }
            }
        }
    }
}
#endif