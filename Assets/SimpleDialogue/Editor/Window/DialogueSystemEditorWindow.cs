#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using SimpleDialogue;

namespace SimpleDialogueEditor
{
    public class DialogueSystemEditorWindow : EditorWindow
    {
        public DialogueSystemStaticOptions DialogueOptions;

        private byte currentLanguageIndex;
        private string[] languageNames;

        private KeyCode activeSubmitKey;
        private KeyCode altSubmitKey;

        private KeyCode activeSkipKey;
        private KeyCode altSkipKey;

        private KeyCode activeSkipToAllKey;
        private KeyCode altSkipToAllKey;

        private float tickTime;
        private Font font;
        private Color color;

        private char[] soundlessChars;
        private IntervalChar[] intervalChars;

        private const string c_base64Icon = "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";
        private static Texture2D _mainIcon;

        private void Awake()
        {
            _mainIcon = new Texture2D(256, 256);
            byte[] iconBytes = System.Convert.FromBase64String(c_base64Icon);
            if (_mainIcon.LoadImage(iconBytes))
            {
                _mainIcon.filterMode = FilterMode.Point;
                _mainIcon.Apply();
            }

            DialogueSystem.OnKeysUpdate += KeysUpdate;
            DialogueSystem.OnSettingsUpdate += SettingsUpdate;
            //DialogueSystem.OnLanguageChange += OnLanguageChange;
        }

        [MenuItem("Tools/Dialogue System Editor")]
        public static void ShowWindow()
        {
            if (!Application.isPlaying)
            {
                DialogueSystemEditorWindow window = GetWindow<DialogueSystemEditorWindow>("Dialogue System");
                window.minSize = new Vector2(520, 230);
                window.titleContent = new GUIContent("Dialogue System", _mainIcon);
                window.position = new Rect(30, 30, 600, 400);
                window.Show();
            }
        }

        public void KeysUpdate()
        {
            OnFieldUpdate();
        }

        public void SettingsUpdate()
        {
            OnFieldUpdate();
        }

        public void OnLanguageChange(byte index)
        {
            OnFieldUpdate();
        }


        private void OnEnable()
        {

            if (!Application.isPlaying)
            {
                DialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                if (DialogueOptions == null)
                {
                    DialogueSystemStaticOptions newOptions = new DialogueSystemStaticOptions();
                    AssetDatabase.CreateAsset(newOptions, "Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                    DialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                }

                soundlessChars = DialogueOptions.SoundlessChars;
                intervalChars = DialogueOptions.IntervalChars;

                tickTime = DialogueOptions.DefaultTickTime;
                font = DialogueOptions.DefaultFont;
                if (font == null)
                {
                    font = AssetDatabase.LoadAssetAtPath<Font>("Assets/SimpleDialogue/Resources/Fonts/DeterminationMono.otf");
                }
                color = DialogueOptions.DefaultColor;

                activeSubmitKey = DialogueOptions.ActiveSubmitKey;
                altSubmitKey = DialogueOptions.AltSubmitKey;

                activeSkipKey = DialogueOptions.ActiveSkipKey;
                altSkipKey = DialogueOptions.AltSkipKey;

                activeSkipToAllKey = DialogueOptions.ActiveSkipToAllKey;
                altSkipToAllKey = DialogueOptions.AltSkipToAllKey;

                //currentLanguageIndex = DialogueOptions.CurrentLanguageIndex;
                //languageNames = DialogueOptions.LanguageNames;

                isLanguageEditing = false;
            }
            else
            {
                //languageNames = DialogueSystem.GetLanguagesArray();
                //currentLanguageIndex = DialogueSystem.LanguageIndex;
            }
        }

        private void OnFieldUpdate()
        {
            OnGUI();
        }

        private bool isLanguageEditing;

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {

               /* if (!isLanguageEditing)
                {
                    ViewLanguage();
                    EditorGUILayout.Space(3f);
                    if (EditorGUILayout.LinkButton("Edit Languages"))
                    {
                        isLanguageEditing = true;
                        titleContent = new GUIContent("Language Editing", new Texture2D(0, 0));
                    }
                }
                else
                {
                    ViewLanguageList();
                    EditorGUILayout.Space(3f);
                    if (EditorGUILayout.LinkButton("Exit Editing"))
                    {
                        isLanguageEditing = false;
                        titleContent = new GUIContent("Dialogue System", _mainIcon);
                    }
                } */

                if (!isLanguageEditing)
                {
                    EditorGUILayout.Space(5f);
                    EditorGUILayout.LabelField("Input", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal();
                    activeSubmitKey = (KeyCode)EditorGUILayout.EnumPopup("Active Submit Key", activeSubmitKey, EditorStyles.popup, GUILayout.Height(20));
                    altSubmitKey = (KeyCode)EditorGUILayout.EnumPopup("Alt Submit Key", altSubmitKey, EditorStyles.popup, GUILayout.Height(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(3f);
                    EditorGUILayout.BeginHorizontal();
                    activeSkipKey = (KeyCode)EditorGUILayout.EnumPopup("Active Skip Key", activeSkipKey, EditorStyles.popup, GUILayout.Height(20));
                    altSkipKey = (KeyCode)EditorGUILayout.EnumPopup("Alt Skip Key", altSkipKey, EditorStyles.popup, GUILayout.Height(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(3f);
                    EditorGUILayout.BeginHorizontal();
                    activeSkipToAllKey = (KeyCode)EditorGUILayout.EnumPopup("Active SkipToAll Key", activeSkipToAllKey, EditorStyles.popup, GUILayout.Height(20));
                    altSkipToAllKey = (KeyCode)EditorGUILayout.EnumPopup("Alt SkipToAll Key", altSkipToAllKey, EditorStyles.popup, GUILayout.Height(20));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(5f);
                    EditorGUILayout.LabelField("Default Settings", EditorStyles.boldLabel);

                    tickTime = EditorGUILayout.Slider("Tick Time", tickTime, 0.0015f, 0.3f);
                    font = (Font)EditorGUILayout.ObjectField("Font", font, typeof(Font), false);
                    color = EditorGUILayout.ColorField("Color", color);

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.LabelField("SoundLess Chars", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal();
                    for (int i = 0; i < soundlessChars.Length; i++)
                    {
                        soundlessChars[i] = EditorGUILayout.TextField(soundlessChars[i].ToString())[0];
                    }

                    if (GUILayout.Button("Add", GUILayout.Width(50)))
                    {
                        char lastChar = soundlessChars[soundlessChars.Length - 1];
                        List<char> soundlessCharsList = new List<char>(soundlessChars);
                        soundlessCharsList.Add(lastChar);
                        soundlessChars = soundlessCharsList.ToArray();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("Interval Chars", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUILayout.BeginVertical(GUILayout.Height(50));
                    EditorGUILayout.LabelField("Target Char", GUILayout.Width(80));
                    EditorGUILayout.LabelField("Interval Scale", GUILayout.Width(80));
                    EditorGUILayout.EndVertical();

                    for (int i = 0; i < intervalChars.Length; i++)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(50));
                        intervalChars[i].Char = EditorGUILayout.TextField(intervalChars[i].Char.ToString(), GUILayout.ExpandHeight(true))[0];
                        intervalChars[i].IntervalScale = EditorGUILayout.Slider(intervalChars[i].IntervalScale, 1f, 15f, GUILayout.ExpandHeight(true));
                        EditorGUILayout.EndVertical();

                    }

                    if (GUILayout.Button("Add", GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        IntervalChar lastChar = intervalChars[intervalChars.Length - 1];
                        List<IntervalChar> intervalsCharsList = new List<IntervalChar>(intervalChars);
                        intervalsCharsList.Add(lastChar);
                        intervalChars = intervalsCharsList.ToArray();
                    }


                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(15f);

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Reset chars options", GUILayout.Width(140)))
                    {
                        DialogueOptions.SoundlessChars = new char[4];
                        DialogueOptions.SoundlessChars[0] = ' ';
                        DialogueOptions.SoundlessChars[1] = '.';
                        DialogueOptions.SoundlessChars[2] = ',';
                        DialogueOptions.SoundlessChars[3] = '?';

                        DialogueOptions.IntervalChars = new IntervalChar[3];
                        DialogueOptions.IntervalChars[0] = new IntervalChar(',', 5f);
                        DialogueOptions.IntervalChars[1] = new IntervalChar('.', 8f);
                        DialogueOptions.IntervalChars[2] = new IntervalChar('!', 8f);

                        soundlessChars = DialogueOptions.SoundlessChars;
                        intervalChars = DialogueOptions.IntervalChars;
                    }
                    GUILayout.Space(12f);
                    EditorGUILayout.EndHorizontal();
                }
                

                //DialogueOptions.CurrentLanguageIndex = currentLanguageIndex;
                //DialogueOptions.LanguageNames = languageNames;
                DialogueOptions.ActiveSubmitKey = activeSubmitKey;
                DialogueOptions.AltSubmitKey = altSubmitKey;
                DialogueOptions.ActiveSkipKey = activeSkipKey;
                DialogueOptions.AltSkipKey = altSkipKey;
                DialogueOptions.ActiveSkipToAllKey = activeSkipToAllKey;
                DialogueOptions.AltSkipToAllKey = altSkipToAllKey;
                DialogueOptions.DefaultTickTime = tickTime;
                DialogueOptions.DefaultFont = font;
                DialogueOptions.DefaultColor = color;
                DialogueOptions.SoundlessChars = soundlessChars;
                DialogueOptions.IntervalChars = intervalChars;

            }
            else
            {
                //ViewLanguage();
                //DialogueSystem.SetLanguageOptions(languageNames, currentLanguageIndex);
                EditorGUILayout.Space(5f);
                EditorGUILayout.LabelField("Input", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                DialogueSystem.ActiveSubmitKey = (KeyCode)EditorGUILayout.EnumPopup("Active Submit Key", DialogueSystem.ActiveSubmitKey);
                DialogueSystem.AltSubmitKey = (KeyCode)EditorGUILayout.EnumPopup("Alt Submit Key", DialogueSystem.AltSubmitKey);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(3f);
                EditorGUILayout.BeginHorizontal();
                DialogueSystem.ActiveSkipKey = (KeyCode)EditorGUILayout.EnumPopup("Active Skip Key", DialogueSystem.ActiveSkipKey);
                DialogueSystem.AltSkipKey = (KeyCode)EditorGUILayout.EnumPopup("Alt Skip Key", DialogueSystem.AltSkipKey);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(3f);
                EditorGUILayout.BeginHorizontal();
                DialogueSystem.ActiveSkipToAllKey = (KeyCode)EditorGUILayout.EnumPopup("Active SkipToAll Key", DialogueSystem.ActiveSkipToAllKey);
                DialogueSystem.AltSkipToAllKey = (KeyCode)EditorGUILayout.EnumPopup("Alt SkipToAll Key", DialogueSystem.AltSkipToAllKey);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(15f);
                EditorGUILayout.LabelField("Default Settings", EditorStyles.boldLabel);
                DialogueSystem.DefaultSettings.TickTime = tickTime = EditorGUILayout.Slider("Tick Time", DialogueSystem.DefaultSettings.TickTime, 0.0015f, 0.3f);
                DialogueSystem.DefaultSettings.Font = (Font)EditorGUILayout.ObjectField("Font", DialogueSystem.DefaultSettings.Font, typeof(Font), false);
                DialogueSystem.DefaultSettings.Color = EditorGUILayout.ColorField("Color", DialogueSystem.DefaultSettings.Color);

                GUI.enabled = false;

                soundlessChars = DialogueSystem.SoundlessChars;
                intervalChars = DialogueSystem.IntervalChars;

                EditorGUILayout.Space(5f);

                EditorGUILayout.LabelField("SoundLess Chars", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < soundlessChars.Length; i++)
                {
                    EditorGUILayout.TextField(soundlessChars[i].ToString());
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Interval Chars", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical(GUILayout.Height(50));
                EditorGUILayout.LabelField("Target Char", GUILayout.Width(80));
                EditorGUILayout.LabelField("Interval Scale", GUILayout.Width(80));
                EditorGUILayout.EndVertical();

                for (int i = 0; i < intervalChars.Length; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(50));
                    EditorGUILayout.TextField(intervalChars[i].Char.ToString(), GUILayout.ExpandHeight(true));
                    EditorGUILayout.Slider(intervalChars[i].IntervalScale, 1f, 15f, GUILayout.ExpandHeight(true));
                    EditorGUILayout.EndVertical();

                }

                GUILayout.EndHorizontal();

                GUI.enabled = true;
            }
        }

        private void ViewLanguage()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Language", EditorStyles.boldLabel, GUILayout.Width(130));
            GUILayout.FlexibleSpace();
            string leftLabelText = $"{currentLanguageIndex} - {languageNames[currentLanguageIndex]}";
            EditorGUILayout.LabelField(leftLabelText, GUILayout.Width(leftLabelText.Length * 8));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            for (byte i = 0; i < languageNames.Length; i++)
            {
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

                if (i == currentLanguageIndex)
                {
                    buttonStyle.normal = new GUIStyleState
                    {
                        background = EditorStyles.toggleGroup.active.background,
                        textColor = Color.white
                    };
                    buttonStyle.hover = new GUIStyleState
                    {
                        background = EditorStyles.toggleGroup.active.background,
                        textColor = Color.white
                    };
                    buttonStyle.active = new GUIStyleState
                    {
                        background = EditorStyles.toggleGroup.active.background,
                        textColor = Color.white
                    };
                }
                else
                {
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
                }

                if (GUILayout.Button(languageNames[i], buttonStyle))
                {
                    currentLanguageIndex = i; 
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void ViewLanguageList()
        {
            GUILayout.Space(10f);

            for (byte i = 0; i < languageNames.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string outLanguageName = EditorGUILayout.TextField("Name", languageNames[i], GUILayout.Width(275));
                int lengthToCutout = outLanguageName.Length;
                if (lengthToCutout > 12) lengthToCutout = 12;
                languageNames[i] = outLanguageName.Substring(0, lengthToCutout);
                GUI.enabled = false;
                EditorGUILayout.IntField("Index", i, GUILayout.ExpandWidth(true), GUILayout.MinWidth(170));
                GUI.enabled = true;
                if (languageNames.Length > 1)
                if (GUILayout.Button("remove", GUILayout.ExpandWidth(true)))
                {
                    languageNames = RemoveAt(languageNames, i);
                    currentLanguageIndex = 0;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add new language"))
            {
                List<string> list = new List<string>(languageNames);
                list.Add("NA");
                languageNames = list.ToArray();
            }
        }

        string[] RemoveAt(string[] array, int indexToRemove)
        {
            string[] newArray = new string[array.Length - 1];

            for (int i = 0, j = 0; i < array.Length; i++)
            {
                if (i == indexToRemove)
                    continue; 

                newArray[j] = array[i];
                j++;
            }

            return newArray;
        }
    }
}
#endif