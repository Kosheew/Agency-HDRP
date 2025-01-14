using System;
using UnityEngine;

namespace SimpleDialogue
{
    [Serializable]
    public class DialogueMessage
    {
        public string Text;
        public Sprite SpeakerIcon;
        public AudioClip SpeakerSound;

        [Serializable]
        public struct MessageOverrides
        {
            public bool DoTickTime;
            public float TickTime;

            public bool InstantTick;

            public bool DoMessageFont;
            public Font MessageFont;

            public bool DoDefaultTextColor;
            public Color DefaultTextColor;

            public enum MessageSoundMode
            {
                None = 0,
                OnStart = 1,
                ByChar = 2
            }
            public MessageSoundMode SoundMode;

            public bool PlaySoundlessChars;

            public enum FinishMessageMode
            {
                Default = 0,
                Automaticaly = 1,
                Fixed = 2
            }
            public FinishMessageMode FinishMessage;
            public float AutomaticalyDelay;
            public bool CantSkip;

            public bool DoDefaultTextSize;
            public int DefaultTextSize;
        }

        [Serializable]
        public struct MessageRelativeFormats
        {
            [Serializable]
            public struct RelativeColorFormat
            {
                public int Start;
                public int End;

                public Color Format;

                public bool Gradient;
                public Gradient GradientFormat;
            }

            [Serializable]
            public struct RelativeSizeFormat
            {
                public int Start;
                public int End;

                public int Format;

                public bool Lerp;
                public int FormatOnStart;
                public int FormatOnEnd;
            }

            [Serializable]
            public struct RelativeFontStyleFormat
            {
                public int Start;
                public int End;

                public FontStyle Format;
            }

            public RelativeColorFormat[] RelativeColors;
            public RelativeSizeFormat[] RelativeSizes;
            public RelativeFontStyleFormat[] RelativeFontstyles;
        }

        [Serializable]
        public struct MessageDynamicSources
        {
            public bool DynamicSprite;
            public Sprite[] SpriteLoop;
            public Sprite LastSpriteInLoop;
            public int SpriteLoopOffset;

            public bool DynamicSound;
            public AudioClip[] ChangingSounds;
            public ChangingSoundMode ChangingSoundsMode;

            public enum ChangingSoundMode
            {
                Loop = 0,
                Random = 1,
                UnrepeatableRandom = 2
            }
        }

        public MessageOverrides Overrides;
        public MessageRelativeFormats RelativeFormats;
        public MessageDynamicSources DynamicSources;

        //GET PARAMETERS
        public string[] GetTextArray()
        {
            string text = Text;
            char[] charArray = text.ToCharArray();
            string[] outText = new string[charArray.Length];
            if (charArray != null)
            for (int i = 0; i < charArray.Length; i++) outText[i] = $"{charArray[i]}";

            for (int i = 0; RelativeFormats.RelativeColors.Length > i; i++)
            {
                MessageRelativeFormats.RelativeColorFormat relativeFormat = RelativeFormats.RelativeColors[i];

                for (int charindex = relativeFormat.Start; (relativeFormat.End + 1) > charindex; charindex++)
                {
                    Color currentColor = relativeFormat.Format;
                    if (relativeFormat.Gradient)
                    {
                        float t = (charindex - (relativeFormat.Start)) / (float)((relativeFormat.End + 1) - relativeFormat.Start);
                        currentColor = relativeFormat.GradientFormat.Evaluate(t);
                    }
                    outText[charindex] = this.AddColorFormat(outText[charindex], currentColor);
                }
            }

            for (int i = 0; RelativeFormats.RelativeSizes.Length > i; i++)
            {
                MessageRelativeFormats.RelativeSizeFormat relativeFormat = RelativeFormats.RelativeSizes[i];

                for (int charindex = relativeFormat.Start; (relativeFormat.End + 1) > charindex; charindex++)
                {

                    float currentSize = relativeFormat.Format;
                    if (relativeFormat.Lerp)
                    {
                            float t = (charindex - (relativeFormat.Start)) / (float)((relativeFormat.End + 1) - relativeFormat.Start);
                            currentSize = Mathf.Lerp(relativeFormat.FormatOnStart, relativeFormat.FormatOnEnd, t);
                    }
                    outText[charindex] = this.AddSizeFormat(outText[charindex], currentSize);
                }
            }

            for (int i = 0; RelativeFormats.RelativeFontstyles.Length > i; i++)
            {
                MessageRelativeFormats.RelativeFontStyleFormat relativeFormat = RelativeFormats.RelativeFontstyles[i];

                for (int charindex = relativeFormat.Start; (relativeFormat.End + 1) > charindex; charindex++)
                {
                    outText[charindex] = this.AddFontStyleFormat(outText[charindex], relativeFormat.Format);
                }
            }

            return outText;
        }

       /* private string GetLanguageText()
        {
            string currentText = Text[DialogueSystem.LanguageIndex];
            if (currentText != null)
            {
                return currentText;
            }
            else
            {
                return Text[0];
            }
        } */

        private string AddColorFormat(string charText, Color color)
        {
            string HEX = 
                ((byte)(color.r * 255)).ToString("X2") +
                ((byte)(color.g * 255)).ToString("X2") +
                ((byte)(color.b * 255)).ToString("X2") +
                ((byte)(color.a * 255)).ToString("X2");
            return $"<color=#{HEX}>{charText}</color>";
        }

        private string AddSizeFormat(string charText, float size)
        {
            int intsize = Mathf.RoundToInt(size);
            return $"<size={intsize}>{charText}</size>";
        }

        private string AddFontStyleFormat(string charText, FontStyle style)
        {
            switch (style)
            {
                default:
                    return charText;
                case FontStyle.Bold:
                    return $"<b>{charText}</b>";
                case FontStyle.Italic:
                    return $"<i>{charText}</i>";
                case FontStyle.BoldAndItalic:
                    return $"<b><i>{charText}</i></b>";
            }
        }

        // CONSTRUCTOR
        public DialogueMessage(string Text, Sprite SpeakerIcon, AudioClip SpeakerSound, byte languageCount)
        {
           /* this.Text = new string[Text.Length];
            for (int i = 0; i < Text.Length; i++)
            {
                this.Text[i] = Text[i];
            } */
            this.Text = Text;
            this.SpeakerIcon = SpeakerIcon;
            this.SpeakerSound = SpeakerSound;
            this.Overrides = new MessageOverrides() { TickTime = 0.15f, SoundMode = MessageOverrides.MessageSoundMode.ByChar, FinishMessage = MessageOverrides.FinishMessageMode.Default, AutomaticalyDelay = 1f, DefaultTextColor = Color.white, DefaultTextSize = 50, };
            this.RelativeFormats = new MessageRelativeFormats();
            this.DynamicSources = new MessageDynamicSources() { ChangingSoundsMode = MessageDynamicSources.ChangingSoundMode.Loop };
        }

        // DOUBLE CONSTRUCTOR
        public DialogueMessage(DialogueMessage reference)
        {
           /* this.Text = new string[reference.Text.Length];
            for (int i = 0; i < reference.Text.Length; i++)
            {
                this.Text[i] = reference.Text[i];
            } */
            this.Text = reference.Text;
            this.SpeakerIcon = reference.SpeakerIcon;
            this.SpeakerSound = reference.SpeakerSound;
            this.Overrides = reference.Overrides;
            this.RelativeFormats = reference.RelativeFormats;
            this.DynamicSources = reference.DynamicSources;
        }

        // EMPTY CONSTRUCTOR
        public DialogueMessage()
        {
            this.Text = "";
            this.SpeakerIcon = null;
            this.SpeakerSound = null;
            this.Overrides = new MessageOverrides() { TickTime = 0.15f, SoundMode = MessageOverrides.MessageSoundMode.ByChar, FinishMessage = MessageOverrides.FinishMessageMode.Default, AutomaticalyDelay = 1f, DefaultTextColor = Color.white, DefaultTextSize = 50, };
            this.RelativeFormats = new MessageRelativeFormats();
            this.DynamicSources = new MessageDynamicSources() { ChangingSoundsMode = MessageDynamicSources.ChangingSoundMode.Loop };
        }
    }
}