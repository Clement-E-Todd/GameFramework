﻿using ClementTodd.NodeEvents;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    public class PlayerCharacter : CharacterControl
    {
        public PlayerCharacterControlState playerInputState;
        public ComputerCharacterControlState scriptedCharacterState;

        private void OnEnable()
        {
            if (NodeEventManager.Instance)
            {
                NodeEventManager.Instance.OnEventStarted += OnDialogueStarted;
                NodeEventManager.Instance.OnEventEnded += OnDialogueEnded;
            }
        }

        private void OnDisable()
        {
            if (NodeEventManager.Instance)
            {
                NodeEventManager.Instance.OnEventStarted -= OnDialogueStarted;
                NodeEventManager.Instance.OnEventEnded -= OnDialogueEnded;
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