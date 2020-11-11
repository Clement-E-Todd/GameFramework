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

        private bool textVisible = false;
        private bool dialogueBoxVisible = false;
        private bool nameBoxVisible = false;
        private bool optionsBoxVisible = false;

        private struct CharacterAnimationData
        {
            public float fadeInDelay;
            public float fadeInDuration;
        }
        public float defaultFadeInDelay = 0.01f;
        public float defaultFadeInDuration = 0.01f;
        private float characterAnimationStartTime;
        private CharacterAnimationData[] characterAnimationData;
        const float fadeOutDuration = 0.1f;

        private void Awake()
        {
            Color32 textColor = dialogueBoxLabel.color;
            textColor.a = 0;
            dialogueBoxLabel.color = textColor;
        }

        private void Update()
        {
            UpdateTextAnimation();
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
            dialogueBoxLabel.ForceMeshUpdate();

            ShowDialogueBox();
            DoShowTextAnimation();
        }

        private void DoShowTextAnimation()
        {
            textVisible = true;
            characterAnimationStartTime = Time.unscaledTime;
            characterAnimationData = new CharacterAnimationData[dialogueBoxLabel.textInfo.characterCount];

            float totalFadeInDelay = 0f;

            for (int i = 0; i < dialogueBoxLabel.textInfo.characterCount; i++)
            {
                if (i > 0)
                {
                    totalFadeInDelay += defaultFadeInDelay;
                }
                characterAnimationData[i].fadeInDelay = totalFadeInDelay;
                characterAnimationData[i].fadeInDuration = defaultFadeInDuration;
            }
        }

        private void DoHideTextAnimation()
        {
            textVisible = false;
            characterAnimationStartTime = Time.unscaledTime;
        }

        private void UpdateTextAnimation()
        {
            if (characterAnimationData == null)
            {
                return;
            }

            TMP_TextInfo textInfo = dialogueBoxLabel.textInfo;
            Color32[] vertexColors;

            for (int i = 0; i < dialogueBoxLabel.textInfo.characterCount; i++)
            {
                var data = characterAnimationData[i];
                float alpha = 0f;

                if (textVisible)
                {
                    float animationTime = Time.unscaledTime - characterAnimationStartTime - data.fadeInDelay;
                    alpha = Mathf.Clamp01(animationTime / data.fadeInDuration);
                }
                else
                {
                    float animationTime = Time.unscaledTime - characterAnimationStartTime;
                    alpha = 1f - Mathf.Clamp01(animationTime / data.fadeInDuration);
                }

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                vertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Only change the vertex color if the text element is visible.
                if (textInfo.characterInfo[i].isVisible)
                {
                    Color32 color = textInfo.characterInfo[i].color;
                    color.a = (byte)(255 * alpha);

                    vertexColors[vertexIndex + 0] = color;
                    vertexColors[vertexIndex + 1] = color;
                    vertexColors[vertexIndex + 2] = color;
                    vertexColors[vertexIndex + 3] = color;
                }
            }

            dialogueBoxLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        public void HideText()
        {
            textVisible = false;
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