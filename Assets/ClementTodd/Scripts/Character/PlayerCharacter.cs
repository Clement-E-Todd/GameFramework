using UnityEngine;

namespace ClementTodd
{
    public class PlayerCharacter : Character, IDialogueListener
    {
        public PlayerInputState playerInputState;
        public ScriptedCharacterState scriptedCharacterState;

        private void OnEnable()
        {
            DialogueManager.Instance.AddListener(this);
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance)
            {
                DialogueManager.Instance.RemoveListener(this);
            }
        }

        public void OnDialogueStarted()
        {
            if (CurrentState == playerInputState)
            {
                SetState(scriptedCharacterState);
            }
            else
            {
                Debug.LogWarning(name + " was in an unexpected state when dialogue began.");
            }
        }

        public void OnDialogueEnded()
        {
            if (CurrentState == scriptedCharacterState)
            {
                SetState(playerInputState);
            }
            else
            {
                Debug.LogWarning(name + " was in an unexpected state when dialogue ended.");
            }
        }
    }
}