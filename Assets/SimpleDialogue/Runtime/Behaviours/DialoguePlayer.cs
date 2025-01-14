using UnityEngine;
using UnityEngine.Events;

namespace SimpleDialogue
{
    [AddComponentMenu("Dialogues/Dialogue Player")]
    public class DialoguePlayer : MonoBehaviour
    {
        /// <summary>
        /// Dialogue that playing in DialogueSystem Brain by calling Play() method
        /// </summary>
        public Dialogue CurrentDialogue;

        public UnityEvent OnDialogueFinish;

        protected DialogueSystemBrain brain;

        private void Start()
        {
            brain = DialogueSystem.Brain;

            if (brain == null)
            {
                Debug.LogError("There are no DialogueSystem Brain in scene.");
            }

            OnInitilizated();
        }

        protected virtual void OnInitilizated() {}

        /// <summary>
        /// Playing current dialogue stops prewious
        /// </summary>
        public virtual void Play()
        {
            PlayDialogue(false);
        }

        /// <summary>
        /// Playing current dialogue only if prewious is finished
        /// </summary>
        public virtual void TryPlay()
        {
            PlayDialogue(true);
        }

        protected void PlayDialogue(bool isTrying)
        {
            if (brain != null)
            {
                if (CurrentDialogue != null)
                {
                    PlayCurrent(isTrying);
                }
                else
                {
                    Debug.LogError("Can't play dialogue. Current is null");
                }
            }
            else
            {
                Debug.LogError("There are no DialogueSystem Brain in scene.");
            }
        }

        protected void PlayCurrent(bool appreciateCurrent)
        {
            if (appreciateCurrent)
            {
                if (!brain.IsPlaying)
                {
                    brain.OnDialogueEnd = () => OnDialogueFinish.Invoke();
                    brain.PlayDialogue(CurrentDialogue);
                }
            
            }
            else
            {
                brain.OnDialogueEnd = () => OnDialogueFinish.Invoke();
                brain.PlayDialogue(CurrentDialogue);
            }
        }
    }
}