using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialogue
{
    [AddComponentMenu("Dialogues/NPC Dialogue Player")]
    public class NPCDialoguePlayer : DialoguePlayer
    {
        public List<Dialogue> Dialogues = new();
        public List<UnityEvent> FinishEvents = new();

        /// <summary>
        /// Sequence mode. Stopping - stops playing dialogues from PlayNext() after the end of the Sequence. Repeat Last - repeats the last dialogue from PlayNext(). Loop - starts the Sequence from the beginning after reaching the last.
        /// </summary>
        public NPCPlayerSequenceMode SequenceMode = NPCPlayerSequenceMode.Loop;

        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        [SerializeField] 
        private Sprite[] _sprites;
        private Sprite _defaultSprite;

        [SerializeField] 
        private float _loopTick = 0.15f;
        [SerializeField] 
        private bool _animatedNPCSprite = true;
        private bool _canTalkNPCSprite = true;
        private int _currentDialogueIndex;
        private int _currentSpriteIndex;
        private bool _correctFields;

        private void Reset()
        {
            Dialogues.Clear();
            Dialogues.Add(null);

            FinishEvents.Clear();
            FinishEvents.Add(new());

            _animatedNPCSprite = true;
        }

        protected override void OnInitilizated()
        {
            brain = DialogueSystem.Brain;

            _currentDialogueIndex = 0;
            _spriteLoopCoroutine = SpriteLoopCoroutine();
            if (_animatedNPCSprite)
            {
                if (_spriteRenderer != null)
                {
                    _defaultSprite = _spriteRenderer.sprite;
                }
                else
                {
                    Debug.LogError("There are no Sprite Renderer in field.");
                    _correctFields = false;
                    return;
                }
                if (_sprites != null)
                {
                    if (_sprites.Length > 0)
                    {
                        _correctFields = true;
                    }
                    else
                    {
                        Debug.LogError("Sprite array is empty");
                        _correctFields = false;
                        return;
                    }
                }
                else
                {
                    Debug.LogError("Sprite array is empty");
                    _correctFields = false;
                    return;
                }
            }
            else
            {
                _correctFields = true;
            }
        }

        public override void Play()
        {
            PlayNext(false);
        }

        public override void TryPlay()
        {
            PlayNext(true);
        }

        /// <summary>
        /// Plays the next dialogue by Sequence
        /// </summary> 
        public void PlayNext()
        {
            PlayNext(true);
        }

        public void PlayNext(bool isTrying)
        {
            if (_correctFields)
            {
                if (_canTalkNPCSprite || SequenceMode != NPCPlayerSequenceMode.Stopping)
                {
                    base.CurrentDialogue = Dialogues[_currentDialogueIndex];
                    base.OnDialogueFinish = FinishEvents[_currentDialogueIndex];
                    base.PlayDialogue(isTrying);
                    if (_animatedNPCSprite)
                    {
                        StopCoroutine(_spriteLoopCoroutine);
                        _spriteLoopCoroutine = SpriteLoopCoroutine();
                        StartCoroutine(_spriteLoopCoroutine);
                        brain.OnDialogueEnd += () => { StopCoroutine(_spriteLoopCoroutine); _spriteRenderer.sprite = _defaultSprite; };
                    }
                }

                if ( _currentDialogueIndex == Dialogues.Count - 1)
                {
                    switch ((int)SequenceMode)
                    {
                        case 0:
                            _canTalkNPCSprite = false;
                            break;
                        case 1:
                            _canTalkNPCSprite = true;
                            break;
                        case 2:
                            _currentDialogueIndex = 0;
                            _canTalkNPCSprite = true;
                            break;
                    }
                }
                else
                {
                    _currentDialogueIndex++;
                }
            }
            else
            {
                Debug.LogError("Animated NPC fields aren't set correctly.");
            }
        }

        /// <summary>
        /// Plays dialogue by Sequence index
        /// </summary>
        public void PlayIndex(int index)
        {
            if (_correctFields)
            {
                if (index < Dialogues.ToArray().Length)
                {
                    base.CurrentDialogue = Dialogues[index];
                    base.OnDialogueFinish = FinishEvents[index];
                    base.Play();
                    if (_animatedNPCSprite)
                    {
                        StopCoroutine(_spriteLoopCoroutine);
                        _spriteLoopCoroutine = SpriteLoopCoroutine();
                        StartCoroutine(_spriteLoopCoroutine);
                        brain.OnDialogueEnd += () => { StopCoroutine(_spriteLoopCoroutine); _spriteRenderer.sprite = _defaultSprite; };
                    }
                }
                else
                {
                    Debug.LogError($"There are no dialogue on {index} index");
                }
            }
            else
            {
                Debug.LogError("Animated NPC fields aren't set correctly.");
            }
        }

        private IEnumerator _spriteLoopCoroutine;

        private IEnumerator SpriteLoopCoroutine()
        {
            while (true)
            {
                _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
                _currentSpriteIndex++;
                if (_currentSpriteIndex > _sprites.Length - 1)
                    _currentSpriteIndex = 0;
                yield return new WaitForSeconds(_loopTick);
            }
        }
    }
}

