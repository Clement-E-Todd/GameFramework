using UnityEngine;
using TMPro;

namespace ClementTodd_v0_0_1
{
    public class DialogueCanvas : MonoBehaviour, IDialogueListener
    {
        public Animator animator;

        public TextMeshProUGUI dialogueBoxLabel;
        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI[] optionLabels;
        public RectTransform optionHighlight;
        public Animator optionHighlightAnimator;

        private bool dialogueBoxVisible = false;
        private bool nameBoxVisible = false;
        private bool optionsBoxVisible = false;

        private int selectedOptionIndex = 0;
        public float highlightMoveTime = 0.1f;
        private Vector2 highlightMoveVelocity;

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

        private void Update()
        {
            // Dynamically animate the option highlight towards the selected option
            if (optionsBoxVisible)
            {
                Vector2 destination = GetOptionHighlightDestination(); ;
                optionHighlight.anchoredPosition = Vector2.SmoothDamp(
                    optionHighlight.anchoredPosition,
                    destination,
                    ref highlightMoveVelocity,
                    highlightMoveTime);
            }
        }

        public void SetText(string text)
        {
            dialogueBoxLabel.text = text;
            ShowDialogueBox();
        }

        public void SetName(string name)
        {
            nameLabel.text = name;
            ShowNameBox();
        }

        public void ShowDialogueBox()
        {
            if (!dialogueBoxVisible)
            {
                animator.SetTrigger("Show Dialogue Box");
                dialogueBoxVisible = true;
            }
        }

        public void HideDialogueBox()
        {
            if (dialogueBoxVisible)
            {
                animator.SetTrigger("Hide Dialogue Box");
                dialogueBoxVisible = false;
            }
        }

        public void ShowNameBox()
        {
            if (!nameBoxVisible)
            {
                animator.SetTrigger("Show Name Box");
                nameBoxVisible = true;
            }
        }

        public void HideNameBox()
        {
            if (nameBoxVisible)
            {
                animator.SetTrigger("Hide Name Box");
                nameBoxVisible = false;
            }
        }

        public void ShowOptionsBox(int defaultIndex)
        {
            if (!optionsBoxVisible)
            {
                animator.SetTrigger("Show Options Box");
                optionsBoxVisible = true;

                selectedOptionIndex = defaultIndex;
                optionHighlight.anchoredPosition = GetOptionHighlightDestination();
                optionHighlightAnimator.SetBool("Confirmed", false);
            }
        }

        public void HideOptionsBox()
        {
            if (optionsBoxVisible)
            {
                animator.SetTrigger("Hide Options Box");
                optionsBoxVisible = false;

                optionHighlightAnimator.SetBool("Confirmed", true);
            }
        }

        public void SetOptionText(int index, string text)
        {
            optionLabels[index].text = text;
            optionLabels[index].gameObject.SetActive(true);
        }

        public void ClearAllOptions()
        {
            for (int i = 0; i < optionLabels.Length; i++)
            {
                optionLabels[i].gameObject.SetActive(false);
            }
        }

        public void SetSelectedOptionIndex(int index)
        {
            selectedOptionIndex = index;
        }

        private Vector2 GetOptionHighlightDestination()
        {
            return new Vector2(
                    optionHighlight.anchoredPosition.x,
                    optionLabels[selectedOptionIndex].rectTransform.anchoredPosition.y);
        }

        public void OnDialogueStarted() { }

        public void OnDialogueEnded()
        {
            HideDialogueBox();
            HideNameBox();
            HideOptionsBox();
        }
    }
}