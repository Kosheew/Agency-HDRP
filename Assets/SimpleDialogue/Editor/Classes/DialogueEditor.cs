#if UNITY_EDITOR
using SimpleDialogue;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : Editor
    {
        private Dialogue dialogueTarget;
        private DialogueSystemStaticOptions dialogueOptions;

        private byte editorMode;

        private GUIStyle foldoutStyle;
        private bool foldoutOverride;
        private bool foldoutRelative;
        private bool foldoutDynamic;
        private Color foldoutColorNormal = new Color(0.17f, 0.17f, 0.17f);
        private Color foldoutColorActive = new Color(0.3f, 0.3f, 0.3f);

        private void OnEnable()
        {
            dialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
            if (dialogueOptions == null)
            {
                DialogueSystemStaticOptions newOptions = new DialogueSystemStaticOptions();
                AssetDatabase.CreateAsset(newOptions, "Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                dialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
            }
            editorMode = 0;

            foldoutOverride = true;
            foldoutRelative = false;
            foldoutDynamic = false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            dialogueTarget = (Dialogue)target;
            foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                margin = new RectOffset(10, 10, 5, 5),
                padding = new RectOffset(15, 0, 5, 5),
                normal = { textColor = new(0.8f, 0.8f, 0.8f) },
                onNormal = { textColor = Color.white },
            };
            if (dialogueTarget != null)
            OnGUI(); 
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space(10);
        }

        private void OnGUI()
        {
            if (dialogueTarget.Messages != null) 
            if (dialogueTarget.Messages.Length == 0)
            {
                dialogueTarget.Messages = new DialogueMessage[1] { new() };
            }

            editorMode = EditorModeChanger(editorMode);
            GUILayout.Space(15f);

            switch (editorMode)
            {
                case 0:
                    DrawSimpleMode();
                    break;
                case 1:
                    DrawMessageMode();
                    break;
                case 2:
                    DrawListMode();
                    break;
            }
        }

        private byte EditorModeChanger(byte current)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Editor Mode", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            GUIStyle pressedButtonStyle = new GUIStyle(GUI.skin.button);

            pressedButtonStyle.normal = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.active.background,
                textColor = new(0.7f, 0.7f, 0.7f)
            };
            pressedButtonStyle.hover = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.active.background,
                textColor = new(0.7f, 0.7f, 0.7f)
            };
            pressedButtonStyle.active = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.active.background,
                textColor = new(0.7f, 0.7f, 0.7f)
            };

            buttonStyle.normal = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.normal.background,
                textColor = Color.white
            };
            buttonStyle.hover = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.normal.background,
                textColor = Color.white
            };
            buttonStyle.active = new GUIStyleState
            {
                background = EditorStyles.toggleGroup.normal.background,
                textColor = Color.white
            };

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();


            GUIStyle[] buttonStyles = new GUIStyle[3];

            for (byte i = 0; i < buttonStyles.Length; i++)
            {
                if (i == current)
                {
                    buttonStyles[i] = pressedButtonStyle;
                }
                else
                {
                    buttonStyles[i] = buttonStyle;
                }
            }

            if (GUILayout.Button("Simple", buttonStyles[0], GUILayout.Height(25), GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)))
            {
                current = 0;
            }
            if (GUILayout.Button("Message", buttonStyles[1], GUILayout.Height(25), GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)))
            {
                current = 1;
            }
            if (GUILayout.Button("List", buttonStyles[2], GUILayout.Height(25), GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)))
            {
                current = 2;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            return current;
        }

        private const byte c_simpleModeRawSize = 54;
        private void DrawSimpleMode()
        {
            if (dialogueTarget.Messages == null || dialogueTarget.Messages.Length == 0)
            {
                dialogueTarget.Messages = new DialogueMessage[0];
            }

            for (int i = 0; i < dialogueTarget.Messages.Length; i++)
            {
                if (i > 0)
                {
                    DrawLine();
                    GUILayout.Space(3f);
                }

                GUILayout.BeginHorizontal();

                dialogueTarget.Messages[i].SpeakerIcon = SpriteFieldWithSquarePreview(dialogueTarget.Messages[i].SpeakerIcon, c_simpleModeRawSize, i);

                if (dialogueTarget.Messages.Length > 0)
                {
                    dialogueTarget.Messages[i].Text = DrawSimpleTextArea(dialogueTarget.Messages[i].Text);
                }

                dialogueTarget.Messages[i].SpeakerSound = AudioClipFieldWithSquarePreview(dialogueTarget.Messages[i].SpeakerSound, c_simpleModeRawSize, byte.MaxValue - i);

                GUILayout.EndHorizontal();
                GUILayout.Space(3f);
            }

            if (GUILayout.Button("NEW"))
            {
                List<DialogueMessage> messages = new(dialogueTarget.Messages);

                DialogueMessage newMessage = null;

                if (messages.Count > 0)
                {
                    newMessage = new(messages[messages.Count-1]);
                }
                else
                {
                    newMessage = new();
                }

                messages.Add(newMessage);
    
                dialogueTarget.Messages = messages.ToArray();
            }
        }

        private string DrawSimpleTextArea(string text)
        {
            GUIStyle areaStyle = new(GUI.skin.textField);
            areaStyle.fontSize = 13;
            areaStyle.normal.textColor = new(0.7f, 0.7f, 0.7f);
            areaStyle.hover.textColor = new(0.7f, 0.7f, 0.7f);
            return EditorGUILayout.TextArea(text, areaStyle, GUILayout.Height(c_simpleModeRawSize), GUILayout.ExpandWidth(true));
        }

        private Sprite SpriteFieldWithSquarePreview(Rect position, Sprite obj, GUIStyle style, int controlID)
        {
            if (obj != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(obj);
                if (previewTexture != null)
                {
                    GUI.DrawTexture(position, previewTexture, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                EditorGUI.LabelField(position, "None\n(Sprite)", style);
            }

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown && position.Contains(currentEvent.mousePosition))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(obj, false, "", controlID);
                currentEvent.Use();
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                Object selectedObject = EditorGUIUtility.GetObjectPickerObject();
                if (selectedObject is Sprite selectedSprite)
                {
                    obj = selectedSprite;
                }
                else if (selectedObject == null)
                {
                    obj = null;
                }
            }

            return obj;
        }

        private AudioClip AudioClipFieldWithSquarePreview(Rect position, AudioClip obj, GUIStyle style, int controlID)
        {
            if (obj != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(obj);
                if (previewTexture != null)
                {
                    GUI.DrawTexture(position, previewTexture, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                EditorGUI.LabelField(position, "None\n(AudioClip)", style);
            }

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown && position.Contains(currentEvent.mousePosition))
            {
                EditorGUIUtility.ShowObjectPicker<AudioClip>(obj, false, "", controlID);
                currentEvent.Use();
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                Object selectedObject = EditorGUIUtility.GetObjectPickerObject();
                if (selectedObject is AudioClip selectedAudioClip)
                {
                    obj = selectedAudioClip;
                }
                else if (selectedObject == null)
                {
                    obj = null;
                }
            }

            return obj;
        }

        private Sprite SpriteFieldWithSquarePreview(Sprite obj, int size, int controlID)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, size, GUILayout.Width(size), GUILayout.Height(size));
            GUIStyle style = new(EditorStyles.textField);
            style.normal.textColor = new(0.7f, 0.7f, 0.7f);
            style.fontStyle = FontStyle.Normal;
            style.fontSize = 9;
            style.alignment = TextAnchor.UpperCenter;
            return SpriteFieldWithSquarePreview(rect, obj, style, controlID);
        }

        private AudioClip AudioClipFieldWithSquarePreview(AudioClip obj, int size, int controlID)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, size, GUILayout.Width(size), GUILayout.Height(size));
            GUIStyle style = new(GUI.skin.textField);
            style.normal.textColor = new(0.7f, 0.7f, 0.7f);
            style.fontStyle = FontStyle.Normal;
            style.fontSize = 9;
            style.alignment = TextAnchor.UpperCenter;
            return AudioClipFieldWithSquarePreview(rect, obj, style, controlID);
        }

        private void DrawLine(float height = 1, float? width = null)
        {
            if (width == null)
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(GUILayout.Height(height)), new(0.6f, 0.6f, 0.6f));
            else
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(GUILayout.Height(height), GUILayout.Width((float)width)), new(0.6f, 0.6f, 0.6f));
        }

        private void DrawMessageMode()
        {
            for (int i = 0; i < dialogueTarget.Messages.Length; i++)
            {
                DrawFullMessage(i, showRemoveButton:true);
            }

            if (GUILayout.Button("Add Message"))
            {
                List<DialogueMessage> messages = new(dialogueTarget.Messages);

                DialogueMessage newMessage = null;

                if (messages.Count > 0)
                {
                    newMessage = new(messages[messages.Count - 1]);
                }
                else
                {
                    newMessage = new();
                }

                messages.Add(newMessage);

                dialogueTarget.Messages = messages.ToArray();
            }
        }

        private void DrawFullMessage(int messageIndex, bool messageMode = true, bool showRemoveButton = false)
        {
            DialogueMessage message = dialogueTarget.Messages[messageIndex];

            if (messageMode)
            {
                EditorGUILayout.BeginHorizontal();
                message.SpeakerIcon = (Sprite)EditorGUILayout.ObjectField(message.SpeakerIcon, typeof(Sprite), false, GUILayout.Width(50), GUILayout.Height(50));
                message.Text = EditorGUILayout.TextArea(message.Text, new GUIStyle(GUI.skin.textArea) { fontSize = 13 }, GUILayout.Height(50));
                EditorGUILayout.EndHorizontal();
                message.SpeakerSound = (AudioClip)EditorGUILayout.ObjectField(message.SpeakerSound, typeof(AudioClip), false, GUILayout.Width(150));
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speaker Icon", GUILayout.Width(c_defaultWidth));
                message.SpeakerIcon = (Sprite)EditorGUILayout.ObjectField(message.SpeakerIcon, typeof(Sprite), false);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speaker Sound", GUILayout.Width(c_defaultWidth));
                message.SpeakerSound = (AudioClip)EditorGUILayout.ObjectField(message.SpeakerSound, typeof(AudioClip), false);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text", GUILayout.Width(c_defaultWidth));
                message.Text = EditorGUILayout.TextArea(message.Text);
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10f);

            Rect rect;

            if (messageMode)
            {
                rect = EditorGUILayout.GetControlRect(false, 20);
                if (!foldoutOverride)
                    EditorGUI.DrawRect(rect, foldoutColorNormal);
                else
                    EditorGUI.DrawRect(rect, foldoutColorActive);
                rect = new(rect.x + 3f, rect.y, rect.width - 3f, rect.height);
                foldoutOverride = EditorGUI.Foldout(rect, foldoutOverride, new GUIContent("Overrides"), true, foldoutStyle);
            }
            else
            {
                foldoutOverride = EditorGUILayout.Foldout(foldoutOverride, new GUIContent("Overrides"), true, new GUIStyle(EditorStyles.foldout) { margin = new RectOffset(7, 0, 0, 0)});
            }
            if (foldoutOverride)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);

                if (messageMode)
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                else
                    EditorGUILayout.BeginVertical();

                DrawMessageOverrides(messageIndex);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.Space(3f);

            if (messageMode)
            {
                rect = EditorGUILayout.GetControlRect(false, 20);
                if (!foldoutRelative)
                    EditorGUI.DrawRect(rect, foldoutColorNormal);
                else
                    EditorGUI.DrawRect(rect, foldoutColorActive);
                rect = new(rect.x + 3f, rect.y, rect.width - 3f, rect.height);
                foldoutRelative = EditorGUI.Foldout(rect, foldoutRelative, new GUIContent("Relative Formats"), true, foldoutStyle);
            }
            else
            {
                foldoutRelative = EditorGUILayout.Foldout(foldoutRelative, new GUIContent("Relative Formats"), true, new GUIStyle(EditorStyles.foldout) { margin = new RectOffset(7, 0, 0, 0) });
            }
            if (foldoutRelative)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);

                if (messageMode)
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                else
                    EditorGUILayout.BeginVertical();

                DrawMessageRelativeFormats(messageIndex);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.Space(3f);

            if (messageMode)
            {
                rect = EditorGUILayout.GetControlRect(false, 20);
                if (!foldoutDynamic)
                    EditorGUI.DrawRect(rect, foldoutColorNormal);
                else
                    EditorGUI.DrawRect(rect, foldoutColorActive);
                rect = new(rect.x + 3f, rect.y, rect.width - 3f, rect.height);
                foldoutDynamic = EditorGUI.Foldout(rect, foldoutDynamic, new GUIContent("Dynamic Sources"), true, foldoutStyle);
            }
            else
            {
                foldoutDynamic = EditorGUILayout.Foldout(foldoutDynamic, new GUIContent("Dynamic Sources"), true, new GUIStyle(EditorStyles.foldout) { margin = new RectOffset(7, 0, 0, 0) });
            }
            if (foldoutDynamic)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);

                if (messageMode)
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                else
                    EditorGUILayout.BeginVertical();

                DrawMessageDynamicSources(messageIndex);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.Space(3f);

            if (showRemoveButton)
            {
                DrawLine();
                GUILayout.Space(3f);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Remove", GUILayout.Width(60), GUILayout.Height(20)))
                {
                    List<DialogueMessage> messages = new List<DialogueMessage>(dialogueTarget.Messages);
                    messages.RemoveAt(messageIndex);
                    dialogueTarget.Messages = messages.ToArray();
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(15f);
            }
        }

        private const float c_defaultWidth = 130;
        private void DrawMessageOverrides(int index)
        {
            DialogueMessage message = dialogueTarget.Messages[index];

            EditorGUILayout.BeginHorizontal();
            if (!message.Overrides.InstantTick)
            {
                message.Overrides.DoTickTime = EditorGUILayout.Toggle(message.Overrides.DoTickTime, GUILayout.Width(15));
                GUI.enabled = message.Overrides.DoTickTime;
                EditorGUILayout.LabelField(new GUIContent("Tick time", "tool"), GUILayout.Width(c_defaultWidth));
                message.Overrides.TickTime = EditorGUILayout.Slider(message.Overrides.TickTime, 0.0015f, 0.3f);
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.Toggle(true, GUILayout.Width(15));
                EditorGUILayout.LabelField(new GUIContent("Tick time (Instant)", "tool"), GUILayout.Width(c_defaultWidth));
                EditorGUILayout.Slider(0, 0, 0.3f);
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Instant Tick", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.InstantTick = EditorGUILayout.Toggle(message.Overrides.InstantTick, EditorStyles.radioButton);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            message.Overrides.DoMessageFont = EditorGUILayout.Toggle(message.Overrides.DoMessageFont, GUILayout.Width(15));
            GUI.enabled = message.Overrides.DoMessageFont;
            EditorGUILayout.LabelField(new GUIContent("Font", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.MessageFont = (Font)EditorGUILayout.ObjectField(message.Overrides.MessageFont, typeof(Font), false);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            message.Overrides.DoDefaultTextColor = EditorGUILayout.Toggle(message.Overrides.DoDefaultTextColor, GUILayout.Width(15));
            GUI.enabled = message.Overrides.DoDefaultTextColor;
            EditorGUILayout.LabelField(new GUIContent("Color", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.DefaultTextColor = EditorGUILayout.ColorField(message.Overrides.DefaultTextColor);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Sound Mode", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.SoundMode = (DialogueMessage.MessageOverrides.MessageSoundMode)EditorGUILayout.EnumPopup(message.Overrides.SoundMode);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Play soundless chars", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.PlaySoundlessChars = EditorGUILayout.Toggle(message.Overrides.PlaySoundlessChars, EditorStyles.radioButton, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Finish Message", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.FinishMessage = (DialogueMessage.MessageOverrides.FinishMessageMode)EditorGUILayout.EnumPopup(message.Overrides.FinishMessage);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            if ((int)message.Overrides.FinishMessage > 0)
            {
                EditorGUILayout.LabelField(new GUIContent("Automaticaly Delay", "tool"), GUILayout.Width(c_defaultWidth));
                message.Overrides.AutomaticalyDelay = EditorGUILayout.Slider(message.Overrides.AutomaticalyDelay, 0, 10f);
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField(new GUIContent("Automaticaly Delay", "tool"), GUILayout.Width(c_defaultWidth));
                EditorGUILayout.Slider(0, 0, 10f);
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            if ((int)message.Overrides.FinishMessage < 2)
            {
                EditorGUILayout.LabelField(new GUIContent("Can't skip", "tool"), GUILayout.Width(c_defaultWidth));
                message.Overrides.CantSkip = EditorGUILayout.Toggle(message.Overrides.CantSkip, EditorStyles.radioButton, GUILayout.Width(15));
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField(new GUIContent("Can't skip", "tool"), GUILayout.Width(c_defaultWidth));
                EditorGUILayout.Toggle(true, GUILayout.Width(15));
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            message.Overrides.DoDefaultTextSize = EditorGUILayout.Toggle(message.Overrides.DoDefaultTextSize, GUILayout.Width(15));
            GUI.enabled = message.Overrides.DoDefaultTextSize;
            EditorGUILayout.LabelField(new GUIContent("Text Size", "tool"), GUILayout.Width(c_defaultWidth));
            message.Overrides.DefaultTextSize = EditorGUILayout.IntField(message.Overrides.DefaultTextSize);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMessageRelativeFormats(int index)
        {
            DialogueMessage message = dialogueTarget.Messages[index];

            EditorGUILayout.LabelField("Relative Color Formats", EditorStyles.boldLabel);
            DrawRelativeColorArray(ref message.RelativeFormats.RelativeColors, index);
            EditorGUILayout.LabelField("Relative Size Formats", EditorStyles.boldLabel);
            DrawRelativeSizeArray(ref message.RelativeFormats.RelativeSizes, index);
            EditorGUILayout.LabelField("Relative FontStyle Formats", EditorStyles.boldLabel);
            DrawRelativeFontStyleArray(ref message.RelativeFormats.RelativeFontstyles, index);
        }

        private void DrawRelativeColorArray(ref DialogueMessage.MessageRelativeFormats.RelativeColorFormat[] array, int messageIndex)
        {
            if (array == null) array = new DialogueMessage.MessageRelativeFormats.RelativeColorFormat[0];

            GUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < array.Length; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);

                DrawRelativeColor(messageIndex, i);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    List<DialogueMessage.MessageRelativeFormats.RelativeColorFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeColorFormat>(array);
                    list.RemoveAt(i);
                    array = list.ToArray();
                    break;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(5); 
            }

            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                List<DialogueMessage.MessageRelativeFormats.RelativeColorFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeColorFormat>(array);

                if (array.Length > 0)
                {
                    list.Add(array[array.Length - 1]);
                }
                else
                {
                    list.Add(new DialogueMessage.MessageRelativeFormats.RelativeColorFormat() { Start = 0, End = 5, Format = Color.white});
                }

                array = list.ToArray();
            }
            GUILayout.EndVertical();
        }

        private void DrawRelativeSizeArray(ref DialogueMessage.MessageRelativeFormats.RelativeSizeFormat[] array, int messageIndex)
        {
            if (array == null) array = new DialogueMessage.MessageRelativeFormats.RelativeSizeFormat[0];

            GUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < array.Length; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);

                DrawRelativeSize(messageIndex, i);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    List<DialogueMessage.MessageRelativeFormats.RelativeSizeFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeSizeFormat>(array);
                    list.RemoveAt(i);
                    array = list.ToArray();
                    break;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                List<DialogueMessage.MessageRelativeFormats.RelativeSizeFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeSizeFormat>(array);

                if (array.Length > 0)
                {
                    list.Add(array[array.Length - 1]);
                }
                else
                {
                    list.Add(new DialogueMessage.MessageRelativeFormats.RelativeSizeFormat() { Start = 0, End = 5, Format = 55 });
                }

                array = list.ToArray();
            }
            GUILayout.EndVertical();
        }

        private void DrawRelativeFontStyleArray(ref DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat[] array, int messageIndex)
        {
            if (array == null) array = new DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat[0];

            GUILayout.BeginVertical(GUI.skin.box);

            for (int i = 0; i < array.Length; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);

                DrawRelativeFontStyle(messageIndex, i);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    List<DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat>(array);
                    list.RemoveAt(i);
                    array = list.ToArray();
                    break;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                List<DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat> list = new List<DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat>(array);

                if (array.Length > 0)
                {
                    list.Add(array[array.Length - 1]);
                }
                else
                {
                    list.Add(new DialogueMessage.MessageRelativeFormats.RelativeFontStyleFormat() { Start = 0, End = 5, Format = FontStyle.Bold });
                }

                array = list.ToArray();
            }
            GUILayout.EndVertical();
        }

        private void DrawRelativeColor(int messageIndex, int elementIndex)
        {
            DialogueMessage message = dialogueTarget.Messages[messageIndex];

            EditorGUILayout.LabelField("Edges");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Start", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeColors[elementIndex].Start = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeColors[elementIndex].Start, 0, message.RelativeFormats.RelativeColors[elementIndex].End);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeColors[elementIndex].End = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeColors[elementIndex].End, message.RelativeFormats.RelativeColors[elementIndex].Start, message.Text.Length - 1);
            EditorGUILayout.EndHorizontal();

            if (message.RelativeFormats.RelativeColors[elementIndex].End > message.Text.Length - 1)
            {
                message.RelativeFormats.RelativeColors[elementIndex].End = message.Text.Length - 1;
            }
            if (message.RelativeFormats.RelativeColors[elementIndex].Start > message.RelativeFormats.RelativeColors[elementIndex].End)
            {
                message.RelativeFormats.RelativeColors[elementIndex].Start = message.RelativeFormats.RelativeColors[elementIndex].End;
            }

            string formatZone = message.Text.Substring(message.RelativeFormats.RelativeColors[elementIndex].Start, (message.RelativeFormats.RelativeColors[elementIndex].End + 1) - message.RelativeFormats.RelativeColors[elementIndex].Start);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prewiew", GUILayout.Width(c_defaultWidth));
            EditorGUILayout.LabelField(formatZone, new GUIStyle(EditorStyles.label) { fontSize = 14, fontStyle = FontStyle.BoldAndItalic});
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5f);

            EditorGUILayout.EndVertical();

            if (!message.RelativeFormats.RelativeColors[elementIndex].Gradient)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Format", GUILayout.Width(c_defaultWidth));
                message.RelativeFormats.RelativeColors[elementIndex].Format = EditorGUILayout.ColorField(message.RelativeFormats.RelativeColors[elementIndex].Format);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Format", GUILayout.Width(c_defaultWidth));
                EditorGUILayout.LabelField("Gradient", GUILayout.Width(c_defaultWidth));
                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Gradient", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeColors[elementIndex].Gradient = EditorGUILayout.Toggle(message.RelativeFormats.RelativeColors[elementIndex].Gradient);
            EditorGUILayout.EndHorizontal();

            if (message.RelativeFormats.RelativeColors[elementIndex].Gradient)
            {
                if (message.RelativeFormats.RelativeColors[elementIndex].GradientFormat == null)
                {
                    message.RelativeFormats.RelativeColors[elementIndex].GradientFormat = new();
                }
                message.RelativeFormats.RelativeColors[elementIndex].GradientFormat = EditorGUILayout.GradientField(message.RelativeFormats.RelativeColors[elementIndex].GradientFormat);
            }
        }

        private void DrawRelativeSize(int messageIndex, int elementIndex)
        {
            DialogueMessage message = dialogueTarget.Messages[messageIndex];

            EditorGUILayout.LabelField("Edges");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Start", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeSizes[elementIndex].Start = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeSizes[elementIndex].Start, 0, message.RelativeFormats.RelativeSizes[elementIndex].End);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeSizes[elementIndex].End = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeSizes[elementIndex].End, message.RelativeFormats.RelativeSizes[elementIndex].Start, message.Text.Length - 1);
            EditorGUILayout.EndHorizontal();

            if (message.RelativeFormats.RelativeSizes[elementIndex].End > message.Text.Length - 1)
            {
                message.RelativeFormats.RelativeSizes[elementIndex].End = message.Text.Length - 1;
            }
            if (message.RelativeFormats.RelativeSizes[elementIndex].Start > message.RelativeFormats.RelativeSizes[elementIndex].End)
            {
                message.RelativeFormats.RelativeSizes[elementIndex].Start = message.RelativeFormats.RelativeSizes[elementIndex].End;
            }

            string formatZone = message.Text.Substring(message.RelativeFormats.RelativeSizes[elementIndex].Start, (message.RelativeFormats.RelativeSizes[elementIndex].End + 1) - message.RelativeFormats.RelativeSizes[elementIndex].Start);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prewiew", GUILayout.Width(c_defaultWidth));
            EditorGUILayout.LabelField(formatZone, new GUIStyle(EditorStyles.label) { fontSize = 14, fontStyle = FontStyle.BoldAndItalic });
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5f);

            EditorGUILayout.EndVertical();

            if (!message.RelativeFormats.RelativeSizes[elementIndex].Lerp)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Format", GUILayout.Width(c_defaultWidth));
                message.RelativeFormats.RelativeSizes[elementIndex].Format = EditorGUILayout.IntField(message.RelativeFormats.RelativeSizes[elementIndex].Format);
                if (message.RelativeFormats.RelativeSizes[elementIndex].Format < 1)
                {
                    message.RelativeFormats.RelativeSizes[elementIndex].Format = 1;
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Format", GUILayout.Width(c_defaultWidth));
                EditorGUILayout.LabelField("Lerp");
                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Lerp", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeSizes[elementIndex].Lerp = EditorGUILayout.Toggle(message.RelativeFormats.RelativeSizes[elementIndex].Lerp);
            EditorGUILayout.EndHorizontal();

            if (message.RelativeFormats.RelativeSizes[elementIndex].Lerp)
            {
                message.RelativeFormats.RelativeSizes[elementIndex].FormatOnStart = EditorGUILayout.IntField(new GUIContent("Format on Start"), message.RelativeFormats.RelativeSizes[elementIndex].FormatOnStart);
                message.RelativeFormats.RelativeSizes[elementIndex].FormatOnEnd = EditorGUILayout.IntField(new GUIContent("Format on End"), message.RelativeFormats.RelativeSizes[elementIndex].FormatOnEnd);

                if (message.RelativeFormats.RelativeSizes[elementIndex].FormatOnStart < 0)
                {
                    message.RelativeFormats.RelativeSizes[elementIndex].FormatOnStart = 0;
                }

                if (message.RelativeFormats.RelativeSizes[elementIndex].FormatOnEnd < 0)
                {
                    message.RelativeFormats.RelativeSizes[elementIndex].FormatOnEnd = 0;
                }
            }
        }

        private void DrawRelativeFontStyle(int messageIndex, int elementIndex)
        {
            DialogueMessage message = dialogueTarget.Messages[messageIndex];

            EditorGUILayout.LabelField("Edges");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Start", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeFontstyles[elementIndex].Start = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeFontstyles[elementIndex].Start, 0, message.RelativeFormats.RelativeFontstyles[elementIndex].End);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeFontstyles[elementIndex].End = EditorGUILayout.IntSlider(message.RelativeFormats.RelativeFontstyles[elementIndex].End, message.RelativeFormats.RelativeFontstyles[elementIndex].Start, message.Text.Length - 1);
            EditorGUILayout.EndHorizontal();

            if (message.RelativeFormats.RelativeFontstyles[elementIndex].End > message.Text.Length - 1)
            {
                message.RelativeFormats.RelativeFontstyles[elementIndex].End = message.Text.Length - 1;
            }
            if (message.RelativeFormats.RelativeFontstyles[elementIndex].Start > message.RelativeFormats.RelativeFontstyles[elementIndex].End)
            {
                message.RelativeFormats.RelativeFontstyles[elementIndex].Start = message.RelativeFormats.RelativeFontstyles[elementIndex].End;
            }

            string formatZone = message.Text.Substring(message.RelativeFormats.RelativeFontstyles[elementIndex].Start, (message.RelativeFormats.RelativeFontstyles[elementIndex].End + 1) - message.RelativeFormats.RelativeFontstyles[elementIndex].Start);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prewiew", GUILayout.Width(c_defaultWidth));
            EditorGUILayout.LabelField(formatZone, new GUIStyle(EditorStyles.label) { fontSize = 14, fontStyle = FontStyle.BoldAndItalic });
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5f);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Format", GUILayout.Width(c_defaultWidth));
            message.RelativeFormats.RelativeFontstyles[elementIndex].Format = (FontStyle)EditorGUILayout.EnumPopup(message.RelativeFormats.RelativeFontstyles[elementIndex].Format);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMessageDynamicSources(int index)
        {
            DialogueMessage message = dialogueTarget.Messages[index];
            SerializedProperty messages = serializedObject.FindProperty("Messages");
            SerializedProperty messageProperty = messages.GetArrayElementAtIndex(index);
            SerializedProperty dynamicSources = messageProperty.FindPropertyRelative("DynamicSources");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Dynamic Sprite", "tool"), GUILayout.Width(c_defaultWidth));
            message.DynamicSources.DynamicSprite = EditorGUILayout.Toggle(message.DynamicSources.DynamicSprite, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            if (message.DynamicSources.DynamicSprite)
            {
                SerializedProperty spriteLoop = dynamicSources.FindPropertyRelative("SpriteLoop");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20f);
                EditorGUILayout.BeginVertical();

                serializedObject.Update();
                EditorGUILayout.PropertyField(spriteLoop, new GUIContent("Sprite Loop", "tool"));
                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Last Sprite in Loop", "tool"));
                message.DynamicSources.LastSpriteInLoop = (Sprite)EditorGUILayout.ObjectField(message.DynamicSources.LastSpriteInLoop, typeof(Sprite), false);
                EditorGUILayout.EndHorizontal();

                if (spriteLoop.arraySize < 2)
                {
                    EditorGUILayout.HelpBox("Sprite Loop must have 2 or more elements.", MessageType.Warning, wide: false);
                }

                message.DynamicSources.SpriteLoopOffset = EditorGUILayout.IntSlider(new GUIContent("Sprite Loop offset", "tool"), message.DynamicSources.SpriteLoopOffset, 0, 5);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(7f);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            EditorGUILayout.LabelField(new GUIContent("Dynamic Sound", "tool"), GUILayout.Width(c_defaultWidth));
            message.DynamicSources.DynamicSound = EditorGUILayout.Toggle(message.DynamicSources.DynamicSound, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            if (message.DynamicSources.DynamicSound)
            {
                SerializedProperty changingSounds = dynamicSources.FindPropertyRelative("ChangingSounds");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20f);
                EditorGUILayout.BeginVertical();

                serializedObject.Update();
                EditorGUILayout.PropertyField(changingSounds, new GUIContent("Changing Sounds", "tool"));
                serializedObject.ApplyModifiedProperties();

                if (message.DynamicSources.ChangingSounds != null) 
                if (message.DynamicSources.ChangingSounds.Length >= 2)
                {
                    bool soundsFilled = true;
                    for (int i = 0; i < message.DynamicSources.ChangingSounds.Length; i++)
                    {
                        if (message.DynamicSources.ChangingSounds[i] == null)
                        {
                            soundsFilled = false;
                            break;
                        }
                    }
                    if (!soundsFilled)
                    {
                        EditorGUILayout.HelpBox("Some elements in Changing Sounds are not set.", MessageType.Warning, wide: false);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Changing Sounds must have 2 or more elements.", MessageType.Warning, wide: false);
                }

                message.DynamicSources.ChangingSoundsMode = (DialogueMessage.MessageDynamicSources.ChangingSoundMode)EditorGUILayout.EnumPopup(new GUIContent("Changing Sounds Mode", "tool"), message.DynamicSources.ChangingSoundsMode);

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndChangeCheck();
        }

        private void DrawListMode()
        {
            GUIStyle miniButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedWidth = 22,
                fixedHeight = 20,
                fontSize = 17,
                fontStyle = FontStyle.Bold,
            };

            GUIStyle miniButtonOffsetStyle = new GUIStyle(miniButtonStyle)
            {
                padding = new RectOffset(0, 0, 4, 0),
            };

            for (int i = 0; i < dialogueTarget.Messages.Length; i++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawFullMessage(i, messageMode:false);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("+", miniButtonStyle))
                {
                    List<DialogueMessage> messages = new List<DialogueMessage>(dialogueTarget.Messages);
                    DialogueMessage newMessage = new DialogueMessage(messages[i]);
                    messages.Insert(i + 1, newMessage);
                    dialogueTarget.Messages = messages.ToArray();
                }

                if (GUILayout.Button("-", miniButtonStyle))
                {
                    if (dialogueTarget.Messages.Length > 1)
                    {
                        List<DialogueMessage> messages = new List<DialogueMessage>(dialogueTarget.Messages);
                        messages.RemoveAt(i);
                        dialogueTarget.Messages = messages.ToArray();
                    }
                }

                if (i == 0)
                {
                    GUI.enabled = false;
                }
                if (GUILayout.Button("˄", miniButtonOffsetStyle))
                {
                    List<DialogueMessage> messages = new List<DialogueMessage>(dialogueTarget.Messages);
                    (messages[i - 1], messages[i]) = (messages[i], messages[i - 1]);
                    dialogueTarget.Messages = messages.ToArray();
                }
                GUI.enabled = true;

                if (i == dialogueTarget.Messages.Length - 1)
                {
                    GUI.enabled = false;
                }
                if (GUILayout.Button("˅", miniButtonOffsetStyle))
                {
                    List<DialogueMessage> messages = new List<DialogueMessage>(dialogueTarget.Messages);
                    (messages[i], messages[i + 1]) = (messages[i + 1], messages[i]);
                    dialogueTarget.Messages = messages.ToArray();
                }
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(20f);
            }

            GUILayout.Space(30f);
        }
    }
}
#endif