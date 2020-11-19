using UnityEngine;

namespace ClementTodd
{
    public class PlayerCharacter : Character
    {
        public HumanCharacterControlState playerInputState;
        public ComputerCharacterControlState scriptedCharacterState;

        private void OnEnable()
        {
            if (DialogueManager.Instance)
            {
                DialogueManager.Instance.OnDialogueStarted += OnDialogueStarted;
                DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
            }
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance)
            {
                DialogueManager.Instance.OnDialogueStarted -= OnDialogueStarted;
                DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;
            }
        }

        public void OnDialogueStarted()
        {
            if (CurrentControlState == playerInputState)
            {
                SetControlState(scriptedCharacterState);
            }
            else
            {
                Debug.LogWarning(name + " was in an unexpected state when dialogue began.");
            }
        }

        public void OnDialogueEnded()
        {
            if (CurrentControlState == scriptedCharacterState)
            {
                SetControlState(playerInputState);
            }
            else
            {
                Debug.LogWarning(name + " was in an unexpected state when dialogue ended.");
            }
        }
    }
}