﻿using UnityEngine;
using UnityEngine.UI;

namespace ClementTodd
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance { get; private set; }

        public Animator animator;

        public Text interactionVerbLabel;
        public Text interactionNounLabel;

        private void Awake()
        {
            instance = this;
        }

        public void ShowInteractionPrompt(string actionName, string prepositionAndNoun)
        {
            interactionVerbLabel.text = actionName;
            interactionNounLabel.text = prepositionAndNoun;

            animator.SetTrigger("Show Interaction Prompt");
        }

        public void HideInteractionPrompt()
        {
            animator.SetTrigger("Hide Interaction Prompt");
        }

        public void OnPlayerInteractionTriggered()
        {
            animator.SetTrigger("Interaction Triggered");
        }
    }
}