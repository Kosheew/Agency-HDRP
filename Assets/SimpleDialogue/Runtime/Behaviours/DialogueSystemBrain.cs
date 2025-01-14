using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace SimpleDialogue
{
    [AddComponentMenu("Dialogues/DialogueSystem Brain", 0)]
    public class DialogueSystemBrain : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystemStaticOptions _dialogueSystemStaticOptions;

        [SerializeField]
        private RectTransform _panel;
        [SerializeField]
        private Image _spriteImage;
        [SerializeField]
        private Text _withSpriteText;
        [SerializeField]
        private Text _withoutSpriteText;

        private Text _activeText;
        private int _defaultFontSize = 50;

        [SerializeField]
        private AudioSource _audioSource;

        public AudioSource ActiveAudioSource { get { return _audioSource; } private set { } }
        public Boolean IsPlaying { get; private set; }

        private void Awake()
        {
            DialogueSystem.SetAllOptions(_dialogueSystemStaticOptions);
            DialogueSystem.SetAllOptions(this);
            IsPlaying = false;
        }

        private Dialogue _currentDialogue;
        private DialogueMessage _currentDialogueMessage;
        private string[] _textArray;
        private int _currentMessageIndex;

        public void PlayDialogue(Dialogue dialogue)
        {
            StopAllCoroutines();

            _currentDialogue = dialogue;
            _currentDialogueMessage = dialogue.Messages[0];
            _currentMessageIndex = 0;

            _withSpriteText.fontSize = _withoutSpriteText.fontSize;
            _defaultFontSize = _withSpriteText.fontSize;

            IsPlaying = true;

            targetWaitForSkipCoroutine = WaitForSkipCoroutine();
            targetEndMessageCoroutine = EndMessageCoroutine();
            targetAutoEndCoroutine = AutoEndCoroutine();
            targetPrintMessageCoroutine = PrintMessageCoroutine();
            StartCoroutine(targetPrintMessageCoroutine);
        }

        public void TryPlayDialogue(Dialogue dialogue)
        {
            if (!IsPlaying)
            {
                PlayDialogue(dialogue);
            }
        }

        // Finish mode
        private int finishMode;
        private float autoDelay;
        private bool cantSkip;

        // Dynamic Sprite index
        private int changeSpriteOffsetIndex;

        private IEnumerator targetPrintMessageCoroutine;
        private IEnumerator targetWaitForSkipCoroutine;
        private IEnumerator targetEndMessageCoroutine;
        private IEnumerator targetAutoEndCoroutine;

        //PRINT_PROCEDURE

        private IEnumerator PrintMessageCoroutine()
        {
            _withSpriteText.text = string.Empty;
            _withoutSpriteText.text = string.Empty;
            // FOR text
            string text = _currentDialogueMessage.Text;
            _textArray = _currentDialogueMessage.GetTextArray();
            // OUT text

            // Change icon
            _panel.gameObject.SetActive(true);
            if (_currentDialogueMessage.SpeakerIcon != null )
            {
                _spriteImage.sprite = _currentDialogueMessage.SpeakerIcon;
                _spriteImage.gameObject.SetActive(true);
                _withSpriteText.gameObject.SetActive(true);
                _withoutSpriteText.gameObject.SetActive(false);
                _activeText = _withSpriteText;
            }
            else
            {
                _spriteImage.gameObject.SetActive(false);
                _withSpriteText.gameObject.SetActive(false);
                _withoutSpriteText.gameObject.SetActive(true);
                _activeText = _withoutSpriteText;
            }

            // Finish mode
            finishMode = (int)_currentDialogueMessage.Overrides.FinishMessage;
            autoDelay = _currentDialogueMessage.Overrides.AutomaticalyDelay;
            cantSkip = _currentDialogueMessage.Overrides.CantSkip;

            // Dynamic Sprite index
            changeSpriteOffsetIndex = 0;

            DialogueMessage.MessageOverrides overrides = _currentDialogueMessage.Overrides;

            // Override font
            if (overrides.DoMessageFont)
            {
                Font font = overrides.MessageFont;
                if (font != null)
                {
                    _withSpriteText.font = font;
                    _withoutSpriteText.font = font;
                }
            }
            else
            {
                Font font = DialogueSystem.DefaultSettings.Font;
                if (font != null)
                {
                    _withSpriteText.font = font;
                    _withoutSpriteText.font = font;
                }
            }

            // Sound mode
            int soundMode = (int)overrides.SoundMode;
            AudioClip originClip = _currentDialogueMessage.SpeakerSound;

            _audioSource.Stop();

            // Change default Text fields
            if (overrides.DoDefaultTextColor)
            {
                _activeText.color = overrides.DefaultTextColor;
            }
            else
            {
                _activeText.color = DialogueSystem.DefaultSettings.Color;
            }
            if (overrides.DoDefaultTextSize)
            {
                _activeText.fontSize = overrides.DefaultTextSize;
            }
            else
            {
                _activeText.fontSize = _defaultFontSize;
            }

            DialogueMessage.MessageDynamicSources dynamicSources = _currentDialogueMessage.DynamicSources;
            int dynamicSoundMode = (int)_currentDialogueMessage.DynamicSources.ChangingSoundsMode;

            // Start printing
            if (!overrides.InstantTick)
            {
                if (!cantSkip)
                {
                    targetWaitForSkipCoroutine = WaitForSkipCoroutine();
                    StartCoroutine(targetWaitForSkipCoroutine);
                }

                if (soundMode == 1)
                {
                    if (originClip != null)
                    {
                        if (ActiveAudioSource != null) ActiveAudioSource.PlayOneShot(originClip);
                    }
                }
                int dynamicAudioClipIndex = 0;
                int prewiousAudioClipIndex = 0;
                int dynamicSpriteIndex = 0;

                // Print chars
                for (int i = 0; i < text.Length; i++)
                {
                    char originChar = text[i];
                    string outChar = _textArray[i];

                    if (dynamicSources.DynamicSprite)
                    {
                        _spriteImage.sprite = dynamicSources.SpriteLoop[dynamicSpriteIndex];

                        if (changeSpriteOffsetIndex < dynamicSources.SpriteLoopOffset)
                        {
                            changeSpriteOffsetIndex++;
                        }
                        else
                        {
                            if (dynamicSpriteIndex < dynamicSources.SpriteLoop.Length - 1)
                            {
                                dynamicSpriteIndex++;
                            }
                            else
                            {
                                dynamicSpriteIndex = 0;
                            }
                            changeSpriteOffsetIndex = 0;
                        }

                    }

                    // Sound by Char
                    if (soundMode == 2)
                    {
                        AudioClip outClip = originClip;

                        bool isSoundless = DialogueSystem.SoundlessChars.Contains(originChar);
                        if (!isSoundless || _currentDialogueMessage.Overrides.PlaySoundlessChars)
                        {
                            // Dynamic sounds
                            if (dynamicSources.DynamicSound)
                            {
                                if (dynamicSources.ChangingSounds.Length > 1)
                                {
                                    if (dynamicSoundMode == 0)
                                    {
                                        int currentIndex = dynamicAudioClipIndex;
                                        outClip = dynamicSources.ChangingSounds[currentIndex];

                                        if (currentIndex == dynamicSources.ChangingSounds.Length - 1)
                                        {
                                            dynamicAudioClipIndex = 0;
                                        }
                                        else
                                        {
                                            dynamicAudioClipIndex++;
                                        }
                                    }
                                    if (dynamicSoundMode == 1)
                                    {
                                        int currentIndex = UnityEngine.Random.Range(0, dynamicSources.ChangingSounds.Length);
                                        outClip = dynamicSources.ChangingSounds[currentIndex];

                                        prewiousAudioClipIndex = currentIndex;
                                    }
                                    if (dynamicSoundMode == 2)
                                    {
                                        int currentIndex = 0;
                                        do
                                        {
                                            currentIndex = UnityEngine.Random.Range(0, dynamicSources.ChangingSounds.Length);
                                        }
                                        while (currentIndex == prewiousAudioClipIndex);

                                        outClip = dynamicSources.ChangingSounds[currentIndex];
                                        prewiousAudioClipIndex = currentIndex;
                                    }
                                }

                            }

                            if (outClip != null)
                            {
                                if (ActiveAudioSource != null) ActiveAudioSource.PlayOneShot(outClip);
                            }
                        }

                    }
                    StringBuilder currentText = new StringBuilder();

                    currentText.Append(outChar);
                    _activeText.text = currentText.ToString();

                    float ticktime = 0f;
                    if (!_currentDialogueMessage.Overrides.DoTickTime)
                    {
                        ticktime = DialogueSystem.DefaultSettings.TickTime;
                    }
                    else
                    {
                        ticktime = _currentDialogueMessage.Overrides.TickTime;
                    }

                    foreach (IntervalChar inch in DialogueSystem.IntervalChars)
                    {
                        if (originChar == inch.Char)
                        {
                            ticktime *= inch.IntervalScale;
                            break;
                        }
                    }

                    yield return new WaitForSeconds(ticktime);
                }
            }
            else
            {
                if (soundMode > 0)
                {
                    if (originClip != null)
                    {
                        if (ActiveAudioSource != null) ActiveAudioSource.PlayOneShot(originClip);
                    }
                }
            }
            yield return null;
            targetEndMessageCoroutine = EndMessageCoroutine();
            StartCoroutine(targetEndMessageCoroutine);
            yield break;
        }

        private IEnumerator WaitForSkipCoroutine()
        {
            yield return null;
            while (!Input.GetKey(DialogueSystem.ActiveSkipKey) && !Input.GetKey(DialogueSystem.AltSkipKey) && !Input.GetKey(DialogueSystem.ActiveSkipToAllKey) && !Input.GetKey(DialogueSystem.AltSkipToAllKey))
            {
                yield return null;
            }
            targetEndMessageCoroutine = EndMessageCoroutine();
            StartCoroutine(targetEndMessageCoroutine);
            yield break;
        }

        private IEnumerator EndMessageCoroutine()
        {
            StopCoroutine(targetWaitForSkipCoroutine);
            StopCoroutine(targetPrintMessageCoroutine);

            _activeText.text = string.Join(string.Empty, _textArray);

            if (_currentDialogueMessage.DynamicSources.DynamicSprite)
            _spriteImage.sprite = _currentDialogueMessage.DynamicSources.LastSpriteInLoop;

            yield return new WaitForFixedUpdate();

            if (finishMode > 0)
            {
                targetAutoEndCoroutine = AutoEndCoroutine();
                StartCoroutine(targetAutoEndCoroutine);
            }

            if (finishMode < 2)
            {
                while (!Input.GetKeyDown(DialogueSystem.ActiveSubmitKey) && !Input.GetKeyDown(DialogueSystem.AltSubmitKey) && !Input.GetKey(DialogueSystem.ActiveSkipToAllKey) && !Input.GetKey(DialogueSystem.AltSkipToAllKey))
                {
                    yield return null;
                }
                DoEndMessage();
            }
            

            yield break;
        }

        private IEnumerator AutoEndCoroutine()
        {
            yield return new WaitForSeconds(autoDelay);
            DoEndMessage();
            yield break;
        }

        private void DoEndMessage()
        {
            StopCoroutine(targetEndMessageCoroutine);
            StopCoroutine(targetAutoEndCoroutine);

            _withSpriteText.fontSize = _defaultFontSize;
            _withoutSpriteText.fontSize = _defaultFontSize;

            if (_currentMessageIndex < _currentDialogue.Messages.Length - 1)
            {
                _currentMessageIndex++;
                _currentDialogueMessage = _currentDialogue.Messages[_currentMessageIndex];
                targetPrintMessageCoroutine = PrintMessageCoroutine();
                StartCoroutine(targetPrintMessageCoroutine);
            }
            else
            {
                EndDialogue();
            }
        }

        public Action OnDialogueEnd;
        
        private void EndDialogue()
        {
            StopAllCoroutines();
            IsPlaying = false;
            _panel.gameObject.SetActive(false);
            OnDialogueEnd?.Invoke();
        }
    }
}