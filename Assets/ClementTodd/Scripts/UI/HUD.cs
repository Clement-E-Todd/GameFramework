using UnityEngine;
using UnityEngine.UI;

namespace ClementTodd_v0_0_1
{
    public class HUD : MonoBehaviour
    {
        public Animator animator;

        public static HUD instance { get; private set; }

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
    }
}