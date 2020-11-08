using UnityEngine;
using TMPro;
using System.Collections;

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

        private void Awake()
        {
            Color32 textColor = dialogueBoxLabel.color;
            textColor.a = 0;
            dialogueBoxLabel.color = textColor;
        }

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

            StopAllCoroutines();
            StartCoroutine(DoAnimatedTextReveal());
        }

        public void HideText()
        {
            StopAllCoroutines();

            TMP_TextInfo textInfo = dialogueBoxLabel.textInfo;
            Color32[] vertexColors;
            Color32 c0 = dialogueBoxLabel.color;

            for (int i = 0; i < dialogueBoxLabel.textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                vertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                c0 = textInfo.characterInfo[i].color;
                c0.a = 0;

                vertexColors[vertexIndex + 0] = c0;
                vertexColors[vertexIndex + 1] = c0;
                vertexColors[vertexIndex + 2] = c0;
                vertexColors[vertexIndex + 3] = c0;
            }

            dialogueBoxLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        private IEnumerator DoAnimatedTextReveal()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            TMP_TextInfo textInfo = dialogueBoxLabel.textInfo;
            Color32[] vertexColors;
            Color32 c0 = dialogueBoxLabel.color;

            for (int i = 0; i < dialogueBoxLabel.textInfo.characterCount; i++)
            {
                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                vertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[i].isVisible)
                {
                    c0 = textInfo.characterInfo[i].color;
                    c0.a = 255;

                    vertexColors[vertexIndex + 0] = c0;
                    vertexColors[vertexIndex + 1] = c0;
                    vertexColors[vertexIndex + 2] = c0;
                    vertexColors[vertexIndex + 3] = c0;
                }

                dialogueBoxLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSecondsRealtime(0.01f);
            }
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
            HideText();
            HideDialogueBox();
            HideNameBox();
            HideOptionsBox();
        }
    }
}