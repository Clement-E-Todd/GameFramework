using UnityEngine;
using TMPro;

namespace ClementTodd_v0_0_1
{
    public class DialogueCanvas : MonoBehaviour, IDialogueListener
    {
        public Animator animator;

        public TextMeshProUGUI dialogueBoxLabel;
        
        public TextMeshProUGUI nameLabel;

        public Menu optionMenu;
        public MenuItem[] options;

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

        public void ShowOptionsBox()
        {
            if (!optionsBoxVisible)
            {
                animator.SetTrigger("Show Options Box");
                optionsBoxVisible = true;

                optionMenu.SetSelectionToDefault();
                optionMenu.EnableSelection();
            }
        }

        public void HideOptionsBox()
        {
            if (optionsBoxVisible)
            {
                animator.SetTrigger("Hide Options Box");
                optionsBoxVisible = false;
            }
        }

        public void SetOptionText(int index, string text)
        {
            options[index].gameObject.SetActive(true);

            TextMeshProUGUI label = options[index].GetComponent<TextMeshProUGUI>();
            label.text = text;
        }

        public void UpdateOptionLinks()
        {
            int highestEnabledIndex = 0;

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].gameObject.activeSelf)
                {
                    highestEnabledIndex = i;
                }
            }
            for (int i = 0; i <= highestEnabledIndex; i++)
            {
                options[i].neighbours.up = options[i > 0 ? i - 1 : highestEnabledIndex];
                options[i].neighbours.down = options[i < highestEnabledIndex ? i + 1 : 0];
            }
        }

        public void ClearAllOptions()
        {
            for (int i = 0; i < options.Length; i++)
            {
                options[i].gameObject.SetActive(false);
            }
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