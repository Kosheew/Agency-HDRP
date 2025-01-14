#if UNITY_EDITOR
using UnityEditor;
using SimpleDialogue;
using UnityEngine;
using System;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(DialogueSystemBrain))]
    public class DialogueSystemBrainEditor : Editor
    {
        private const string c_base64Icon = "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";
        private const FilterMode c_filterMode = FilterMode.Bilinear;
        private Texture2D _iconTexture;

        private DialogueSystemBrain brain;
        private MonoScript monoScript;
        private Boolean initilizated = false;

        private SerializedProperty panelProperty;
        private SerializedProperty spriteImageProperty;
        private SerializedProperty withSpriteTextProperty;
        private SerializedProperty withoutSpriteTextProperty;
        private SerializedProperty audioSourceProperty;
        private SerializedProperty optionsProperty;

        private void Awake()
        {
            brain = (DialogueSystemBrain)target;
            monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)brain);

            byte[] iconBytes = Convert.FromBase64String(c_base64Icon);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(iconBytes))
            {
                texture.filterMode = c_filterMode;
                texture.Apply();
                _iconTexture = texture;
            }

            panelProperty = serializedObject.FindProperty("_panel");
            spriteImageProperty = serializedObject.FindProperty("_spriteImage");
            withSpriteTextProperty = serializedObject.FindProperty("_withSpriteText");
            withoutSpriteTextProperty = serializedObject.FindProperty("_withoutSpriteText");
            audioSourceProperty = serializedObject.FindProperty("_audioSource");
            optionsProperty = serializedObject.FindProperty("_dialogueSystemStaticOptions");

            initilizated = true;
            OnEnable();
        }

        private void OnEnable()
        {
            EditorGUIUtility.SetIconForObject(monoScript, _iconTexture);
        }

        private void OnDisable()
        {
           // EditorGUIUtility.SetIconForObject(target, new Texture2D(0,0));
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
            DialogueSystemBrain brain = (DialogueSystemBrain)target;

            EditorGUILayout.PropertyField(panelProperty, new GUIContent("Dialogue Panel"));
            EditorGUILayout.PropertyField(spriteImageProperty, new GUIContent("Sprite Image"));
            EditorGUILayout.LabelField("Text Elements", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(withSpriteTextProperty, new GUIContent("WithSprite Text"));
            EditorGUILayout.PropertyField(withoutSpriteTextProperty, new GUIContent("WithoutSprite Text"));
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(audioSourceProperty, new GUIContent("Audio Source"));

            DialogueSystemStaticOptions dialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
            if (dialogueOptions == null)
            {
                dialogueOptions = ScriptableObject.CreateInstance<DialogueSystemStaticOptions>();
                AssetDatabase.CreateAsset(dialogueOptions, "Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                AssetDatabase.SaveAssets();
            }

            GUILayout.Space(5f);

            if (dialogueOptions != null)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                /*
                if (!Application.isPlaying)
                    GUILayout.Label($"Current Language: {dialogueOptions.LanguageNames[dialogueOptions.CurrentLanguageIndex]}");
                else
                    GUILayout.Label($"Current Language: {DialogueSystem.Language}");
                */

                if (Application.isPlaying)
                    GUILayout.Label($"Is Playing: {brain.IsPlaying}");
                else
                    GUILayout.Label($"Is Playing: {false}");

                if (Application.isPlaying)
                    GUILayout.Label($"Tick Time: {DialogueSystem.DefaultSettings.TickTime}");
                else
                    GUILayout.Label($"Tick Time: {dialogueOptions.DefaultTickTime}");

                if (Application.isPlaying)
                    GUILayout.Label($"DefaultFont: {DialogueSystem.DefaultSettings.Font.name}");
                else
                    GUILayout.Label($"DefaultFont: {dialogueOptions.DefaultFont.name}");

                GUILayout.Space(3f);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical(GUILayout.Width(130));
                if (Application.isPlaying)
                    GUILayout.Label($"Submit Key: {DialogueSystem.ActiveSubmitKey}");
                else
                    GUILayout.Label($"Submit Key: {dialogueOptions.ActiveSubmitKey}");

                if (Application.isPlaying)
                    GUILayout.Label($"Skip Key: {DialogueSystem.ActiveSkipKey}");
                else
                    GUILayout.Label($"Skip Key: {dialogueOptions.ActiveSkipKey}");

                if (Application.isPlaying)
                    GUILayout.Label($"SkipToAll Key: {DialogueSystem.ActiveSkipToAllKey}");
                else
                    GUILayout.Label($"SkipToAll Key: {dialogueOptions.ActiveSkipToAllKey}");

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(130));
                if (Application.isPlaying)
                    GUILayout.Label($"Alt: {DialogueSystem.AltSubmitKey}");
                else
                    GUILayout.Label($"Alt: {dialogueOptions.AltSubmitKey}");

                if (Application.isPlaying)
                    GUILayout.Label($"Alt: {DialogueSystem.AltSkipKey}");
                else
                    GUILayout.Label($"Alt: {dialogueOptions.AltSkipKey}");

                if (Application.isPlaying)
                    GUILayout.Label($"Alt: {DialogueSystem.AltSkipToAllKey}");
                else
                    GUILayout.Label($"Alt: {dialogueOptions.AltSkipToAllKey}");

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                GUILayout.Space(5f);

                optionsProperty.objectReferenceValue = dialogueOptions;

                GUI.enabled = false;
                EditorGUILayout.PropertyField(optionsProperty, new GUIContent("Dialogue System Options File"));
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.PropertyField(optionsProperty, new GUIContent("Dialogue System Options File"));
                EditorGUILayout.HelpBox("Failed to find Options File", MessageType.Warning);
            }
        }
    }
}
#endif