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

        private bool dialogueBoxVisible = false;
        private bool nameBoxVisible = false;
        private bool optionsBoxVisible = false;

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

        public void SetText(string text)
        {
            dialogueBoxLabel.text = text;
            ShowDialogueBox(true);
        }

        public void SetName(string name)
        {
            nameLabel.text = name;
            ShowNameBox(true);
        }

        public void ShowDialogueBox(bool show)
        {
            if (show && !dialogueBoxVisible)
            {
                animator.SetTrigger("Show Dialogue Box");
                dialogueBoxVisible = true;
            }
            else if (!show && dialogueBoxVisible)
            {
                animator.SetTrigger("Hide Dialogue Box");
                dialogueBoxVisible = false;
            }
        }

        public void ShowNameBox(bool show)
        {
            if (show && !nameBoxVisible)
            {
                animator.SetTrigger("Show Name Box");
                nameBoxVisible = true;
            }
            else if (!show && nameBoxVisible)
            {
                animator.SetTrigger("Hide Name Box");
                nameBoxVisible = false;
            }
        }

        public void ShowOptionsBox(bool show)
        {
            if (show && !optionsBoxVisible)
            {
                animator.SetTrigger("Show Options Box");
                optionsBoxVisible = true;
            }
            else if (!show && optionsBoxVisible)
            {
                animator.SetTrigger("Hide Options Box");
                optionsBoxVisible = false;
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

        public void OnDialogueStarted() { }

        public void OnDialogueEnded()
        {
            ShowDialogueBox(false);
            ShowNameBox(false);
            ShowOptionsBox(false);
        }
    }
}