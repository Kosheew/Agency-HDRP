#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using SimpleDialogue;
using System;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(DialoguePlayer), false)]
    public class DialoguePlayerEditor : Editor
    {
        private const string c_base64Icon = "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";
        private const FilterMode c_filterMode = FilterMode.Point;
        private Texture2D _iconTexture;

        private DialoguePlayer player;
        private MonoScript monoScript;
        private Boolean initilizated;

        private SerializedProperty currentDialogueProperty;
        private SerializedProperty onDialogueFinishProperty;

        private void Awake()
        {
            player = (DialoguePlayer)target;
            monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)player);

            byte[] iconBytes = Convert.FromBase64String(c_base64Icon);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(iconBytes))
            {
                texture.filterMode = c_filterMode;
                texture.Apply();
                _iconTexture = texture;
            }

            currentDialogueProperty = serializedObject.FindProperty("CurrentDialogue");
            onDialogueFinishProperty = serializedObject.FindProperty("OnDialogueFinish");

            initilizated = true;
            OnEnable();
        }

        private void OnEnable()
        {          
            EditorGUIUtility.SetIconForObject(monoScript, _iconTexture);
        }

        private void OnDisable()
        {
            if (target != null) 
            EditorGUIUtility.SetIconForObject(target, new Texture2D(0,0));
        }

        public override void OnInspectorGUI()
        {
            if (initilizated)
            {
                serializedObject.Update();
                this.OnGUI();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(1f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(currentDialogueProperty, new GUIContent("Current Dialogue", "Dialogue that playing in DialogueSystem Brain\nby calling Play() method"));
            EditorGUILayout.ToggleLeft(string.Empty, true, GUILayout.Width(15f));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(onDialogueFinishProperty, new GUIContent("On Dialogue Finish"));

            if (Application.isPlaying)
            {
                GUILayout.Space(7f);
                EditorGUILayout.LabelField("Debug Methods", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal("box");
                if (GUILayout.Button(new GUIContent("Play()", "Playing current dialogue stops prewious")))
                    player.Play();
                if (GUILayout.Button(new GUIContent($"TryPlay()", "Playing current dialogue only if prewious is finished")))
                    player.TryPlay();
                EditorGUILayout.EndHorizontal();
            }
        }

    }
}
#endif