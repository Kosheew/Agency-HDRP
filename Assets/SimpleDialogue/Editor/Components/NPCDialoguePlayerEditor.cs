#if UNITY_EDITOR
using SimpleDialogue;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(NPCDialoguePlayer))]
    public class NPCDialoguePlayerEditor : Editor
    {
        private const string c_base64Icon = "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";
        private const FilterMode c_filterMode = FilterMode.Trilinear;
        private Texture2D _iconTexture;

        private bool _foldoutState;
        private int _playNextIndexValue;

        private NPCDialoguePlayer player;
        private MonoScript monoScript;
        private Boolean initilizated;

        SerializedProperty dialoguesProperty;
        SerializedProperty finishEventsProperty;
        SerializedProperty animatedNPCSpriteProperty;
        SerializedProperty spriteRendererProperty;
        SerializedProperty spritesProperty;
        SerializedProperty loopTickProperty;

        private void Awake()
        {
            player = (NPCDialoguePlayer)target;
            monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)player);

            byte[] iconBytes = Convert.FromBase64String(c_base64Icon);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(iconBytes))
            {
                texture.filterMode = c_filterMode;
                texture.Apply();
                _iconTexture = texture;
            }

            dialoguesProperty = serializedObject.FindProperty("Dialogues");
            finishEventsProperty = serializedObject.FindProperty("FinishEvents");
            animatedNPCSpriteProperty = serializedObject.FindProperty("_animatedNPCSprite");
            spriteRendererProperty = serializedObject.FindProperty("_spriteRenderer");
            loopTickProperty = serializedObject.FindProperty("_loopTick");
            spritesProperty = serializedObject.FindProperty("_sprites");

            initilizated = true;
            OnEnable();
        }

        private void OnEnable()
        {
            EditorGUIUtility.SetIconForObject(monoScript, _iconTexture);
            _foldoutState = true;
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
            _foldoutState = EditorGUILayout.Foldout(_foldoutState, new GUIContent("Dialogue Sequence", "Sequence of dialogues in which, when the dialogue ends, the next PlayNext() starts playing the next one, and so on until it reaches the last group, which will be repeated when the PlayNext() is started\nPlayIndex(int) plays Dialogue by index without Sequence"), true, EditorStyles.toolbarDropDown);

            GUILayout.Space(5f);
            
            if (_foldoutState)
            {
                for (int i = 0; i < dialoguesProperty.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    if (i < dialoguesProperty.arraySize - 1 || i == 0)
                        EditorGUILayout.LabelField($"Dialogue {i}", EditorStyles.boldLabel);
                    else
                        EditorGUILayout.LabelField($"Dialogue {i} (last)", EditorStyles.boldLabel);

                    EditorGUILayout.PropertyField(dialoguesProperty.GetArrayElementAtIndex(i), new GUIContent("Target Dialogue"));
                    GUILayout.Space(5f);
                    EditorGUILayout.PropertyField(finishEventsProperty.GetArrayElementAtIndex(i), new GUIContent("On Dialogue Finish"));

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(20)))
                    {
                        Undo.RecordObject(player, "Remove Dialogue");
                        dialoguesProperty.DeleteArrayElementAtIndex(i);
                        finishEventsProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }

                GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Dialogue Group", GUILayout.Width(200), GUILayout.Height(25)))
                {
                    Undo.RecordObject(player, "Add Dialogue Group");

                    dialoguesProperty.arraySize++;
                    finishEventsProperty.arraySize++;

                    SerializedProperty newDialogue = dialoguesProperty.GetArrayElementAtIndex(dialoguesProperty.arraySize - 1);
                    SerializedProperty newEvent = finishEventsProperty.GetArrayElementAtIndex(finishEventsProperty.arraySize - 1);

                    newDialogue.objectReferenceValue = null;

                    UnityEvent newUnityEvent = new UnityEvent();
                    newUnityEvent.Equals(null);
                    player.FinishEvents.Add(newUnityEvent);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5f);
            }

            GUILayout.Space(2f);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("SequenceMode"), new GUIContent("Sequence Mode", "Sequence mode\r\nStopping - stops playing dialogues from PlayNext() after the end of the Sequence.\r\nRepeat Last - repeats the last dialogue from PlayNext().\r\nLoop - starts the Sequence from the beginning after reaching the last."));

            GUILayout.Space(2f);

            animatedNPCSpriteProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Animated NPC Sprite", "Сhanges the sprites of the SpriteRenderer while the dialogue is playing"), animatedNPCSpriteProperty.boolValue, EditorStyles.radioButton, GUILayout.ExpandHeight(true));
            bool animatedNPCSprite = animatedNPCSpriteProperty.boolValue;
            if (animatedNPCSprite)
            {
                GUILayout.Space(3f);
                if (spriteRendererProperty.objectReferenceValue == null)
                {
                    spriteRendererProperty.objectReferenceValue = player.GetComponent<SpriteRenderer>();
                    if (spriteRendererProperty.objectReferenceValue == null)
                        spriteRendererProperty.objectReferenceValue = player.GetComponentInChildren<SpriteRenderer>();
                }
                EditorGUILayout.PropertyField(spriteRendererProperty, new GUIContent("Target SpriteRenderer", "SpriteRenderer, which will be changed"));
                if (spriteRendererProperty.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("There are no Sprite Renderer in field.", MessageType.Error, false);
                }
                GUILayout.Space(1.5f);
                EditorGUILayout.Slider(loopTickProperty, 0.1f, 0.5f, new GUIContent("Loop Tick", "Sprite change interval"));
                GUILayout.Space(1.5f);
                EditorGUILayout.PropertyField(spritesProperty, new GUIContent("Sprites", "Sprites in Loop"));
                if (spritesProperty.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("Sprite array is empty\nAdd sprites in array loop", MessageType.Error, false);
                }
            }

            if (Application.isPlaying)
            {
                GUILayout.Space(7f);
                EditorGUILayout.LabelField("Debug Methods", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal("box");
                if (GUILayout.Button(new GUIContent("PlayNext()", "Plays the next dialogue by Sequence")))
                    player.PlayNext();
                if (GUILayout.Button(new GUIContent($"PlayIndex({_playNextIndexValue})", "Plays dialogue by Sequence index")))
                    player.PlayIndex(_playNextIndexValue);
                _playNextIndexValue = EditorGUILayout.IntSlider(_playNextIndexValue, 0, player.Dialogues.Count - 1, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
}