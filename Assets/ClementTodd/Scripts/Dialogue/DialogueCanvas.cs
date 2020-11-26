using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ClementTodd
{
    public class DialogueCanvas : MonoBehaviour
    {
        public Animator animator;

        public TextMeshProUGUI dialogueBoxLabel;
        
        public TextMeshProUGUI nameLabel;

        public Menu optionMenu;
        public MenuItem[] options;

        private bool showText = false;
        private bool dialogueBoxVisible = false;
        private bool nameBoxVisible = false;
        private bool optionsBoxVisible = false;

        #region Text Animation Definitions

        private bool typewriterInProgress = false;
        private float typewriterStartTime = float.MinValue;

        private float fadeOutStartTime;
        const float fadeOutDuration = 0.1f;

        private struct TextAnimationData
        {
            public DialogueManager.TextAnimationStyle animationStyle;
            public float fadeInDelay;
            public float fadeInDuration;
        }
        private List<TextAnimationData> textAnimationData;

        public float typewriterCharacterDelay = 0.01f;
        public float typewriterFadeInDuration = 0.01f;

        private struct TextAnimationCommand
        {
            public string command;
            public string[] args;
            public string fullText;

            public TextAnimationCommand(string sourceText, int startIndex)
            {
                command = string.Empty;
                args = null;
                fullText = string.Empty;

                bool readingCommand = false;
                bool readingArgs = false;

                List<string> argList = new List<string>();
                int argIndex = 0;

                for (int sourceIndex = startIndex; sourceIndex < sourceText.Length; sourceIndex++)
                {
                    char character = sourceText[sourceIndex];
                    fullText += character;

                    // Start listening for the command name when '{' is found.
                    if (character == '{')
                    {
                        if (readingCommand || command.Length > 0)
                            Debug.LogError("Tried to start a new command but a command is already being processed,");
                        else if (!readingCommand)
                            readingCommand = true;
                        continue;
                    }

                    // End processing when '}' is found.
                    if (character == '}')
                    {
                        break;
                    }

                    // While reading a command name, record each letter until ':' is found.
                    if (readingCommand)
                    {
                        if (character != ':')
                        {
                            command += character;
                        }
                        else
                        {
                            readingCommand = false;
                            readingArgs = true;
                        }

                        continue;
                    }

                    // While reading arguments, record each letter, seperating arguments with ','.
                    if (readingArgs)
                    {
                        while (argIndex >= argList.Count)
                        {
                            argList.Add(string.Empty);
                        }

                        if (character != ',')
                            argList[argIndex] += character;
                        else
                            argIndex++;

                        continue;
                    }
                }

                args = argList.ToArray();
            }
        }
        #endregion

        private void Awake()
        {
            Color32 textColor = dialogueBoxLabel.color;
            textColor.a = 0;
            dialogueBoxLabel.color = textColor;
        }

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

        private void Update()
        {
            UpdateTextAnimation();
        }

        public void SetText(string text)
        {
            InitCharacterAnimationDataFromText(text);

            dialogueBoxLabel.text = RemoveAnimationTags(text);
            dialogueBoxLabel.ForceMeshUpdate();

            ShowDialogueBox();
            StartTypewriterAnimation();
        }

        public bool TryAdvanceText()
        {
            // Remove all currently-visible text (INCLUDING RICH TEXT TAGS) from displayed line to make the overflow text visible.
            int textIndex, richTextIndex;
            for (textIndex = 0, richTextIndex = 0;
                textIndex < dialogueBoxLabel.textInfo.characterCount && richTextIndex < dialogueBoxLabel.text.Length;
                textIndex++, richTextIndex++)
            {
                // If the characters don't match in the text and rich text, we have hit a special instruction in the rich text, so we skip ahead in the rich text until we have found a match again.
                while (dialogueBoxLabel.textInfo.characterInfo[textIndex].character != dialogueBoxLabel.text[richTextIndex] && richTextIndex < dialogueBoxLabel.text.Length)
                {
                    richTextIndex++;
                }
            }

            if (richTextIndex < dialogueBoxLabel.text.Length)
            {
                // Remove the currently-displayed text from the UI
                dialogueBoxLabel.text = dialogueBoxLabel.text.Remove(0, richTextIndex);
                dialogueBoxLabel.ForceMeshUpdate();

                // Clear the removed text's animation data, reducing the delay time before the remaining letters should be shown
                textAnimationData.RemoveRange(0, textIndex);
                if (textAnimationData.Count > 0)
                {
                    float delay = textAnimationData[0].fadeInDelay;
                    for (int i = 0; i < textAnimationData.Count; i++)
                    {
                        TextAnimationData data = textAnimationData[i];
                        data.fadeInDelay -= delay;
                        textAnimationData[i] = data;
                    }
                }

                // Show the next part of the tex
                ShowDialogueBox();
                StartTypewriterAnimation();

                return true;
            }
            else
            {
                return false;
            }
        }

        private void InitCharacterAnimationDataFromText(string sourceText)
        {
            textAnimationData = new List<TextAnimationData>();

            // Define animation properties shared between multiple characters
            float totalFadeInDelay = 0f;
            float delayMultiplier = 1f;
            float fadeInDuration = typewriterFadeInDuration;
            bool readingRichTextMarkup = false;
            DialogueManager.TextAnimationStyle animationStyle = null;

            for (int sourceIndex = 0; sourceIndex < sourceText.Length; sourceIndex++)
            {
                // Define one-time animation properties for this character
                float pauseDuration = 0f;

                // Process character animation markup
                while (sourceText[sourceIndex] == '{')
                {
                    // Read the command from the source text
                    TextAnimationCommand command = new TextAnimationCommand(sourceText, sourceIndex);

                    // Skip the source index ahead to the end of the command
                    if (command.fullText.Length > 0)
                        sourceIndex += command.fullText.Length;
                    else
                        break;

                    // Process the command:
                    switch (command.command.ToLower())
                    {
                        // Add a one-time pause between characters with the the indicated duration.
                        case "pause":
                            {
                                bool success = (command.args.Length > 0 && float.TryParse(command.args[0], out pauseDuration));
                                if (!success)
                                    Debug.Log("Invalid argument(s) for command 'Pause'.");
                            }
                            break;

                        // Multiply the delay duration between characters until the end of the line (or until this command is called again).
                        case "multiplydelay":
                            {
                                bool success = (command.args.Length > 0 && float.TryParse(command.args[0], out delayMultiplier));
                                if (!success)
                                    Debug.Log("Invalid argument for command 'MultiplyDelay'.");
                            }
                            break;

                        // Multiply the fade-in duration for each character until the end of the line (or until this command is called again).
                        case "fadeintime":
                            {
                                bool success = (command.args.Length > 0 && float.TryParse(command.args[0], out fadeInDuration));
                                if (!success)
                                    Debug.Log("Invalid argument for command 'FadeInTime'.");
                            }
                            break;

                        // Set the animation style for this character and following characters
                        case "animationstyle":
                            {
                                animationStyle = null;
                                if (command.args.Length > 0)
                                {
                                    for (int styleIndex = 0; styleIndex < DialogueManager.Instance.textAnimationStyles.Length; styleIndex++)
                                    {
                                        if (DialogueManager.Instance.textAnimationStyles[styleIndex].name == command.args[0])
                                        {
                                            animationStyle = DialogueManager.Instance.textAnimationStyles[styleIndex];
                                            break;
                                        }
                                    }
                                }
                                if (animationStyle == null)
                                {
                                    Debug.LogWarningFormat("Invalid text animation style '{0}'.", command.args[0]);
                                }
                            }
                            break;

                        // Clear the animation style for this character and following characters
                        case "clearanimationstyle":
                            {
                                animationStyle = null;
                            }
                            break;

                        default:
                            {
                                Debug.LogWarningFormat("Unknown command '{0}'.", command.command);
                            }
                            break;
                    }
                }

                // Skip this character if it is rich text markup
                if (sourceText[sourceIndex] == '<')
                {
                    readingRichTextMarkup = true;
                }
                else if (sourceText[sourceIndex] == '>')
                {
                    readingRichTextMarkup = false;
                    continue;
                }
                if (readingRichTextMarkup)
                {
                    continue;
                }

                // Calculate the delay duration for this character (time between the full text's
                // animation beginning and this character appearing).
                totalFadeInDelay += pauseDuration > 0f ? pauseDuration : typewriterCharacterDelay * delayMultiplier;

                // Create the animation data for this character
                TextAnimationData characterData = new TextAnimationData();

                characterData.animationStyle = animationStyle;
                characterData.fadeInDelay = totalFadeInDelay;
                characterData.fadeInDuration = fadeInDuration;

                textAnimationData.Add(characterData);
            }
        }

        private string RemoveAnimationTags(string sourceText)
        {
            string text = string.Empty;
            bool skip = false;

            for (int sourceIndex = 0; sourceIndex < sourceText.Length; sourceIndex++)
            {
                if (sourceText[sourceIndex] == '{')
                {
                    skip = true;
                }
                else if (sourceText[sourceIndex] == '}')
                {
                    skip = false;
                    continue;
                }

                if (skip)
                {
                    continue;
                }

                text += sourceText[sourceIndex];
            }

            return text;
        }

        private void StartTypewriterAnimation()
        {
            showText = true;
            typewriterInProgress = true;
            typewriterStartTime = Time.unscaledTime;
        }

        private void StartFadeOutAnimation()
        {
            showText = false;
            typewriterInProgress = false;
            fadeOutStartTime = Time.unscaledTime;
        }

        public bool TrySkipTypewriterAnimation()
        {
            if (typewriterInProgress)
            {
                typewriterInProgress = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateTextAnimation()
        {
            dialogueBoxLabel.ForceMeshUpdate();

            TMP_TextInfo textInfo = dialogueBoxLabel.textInfo;
            Vector3[] vertices = dialogueBoxLabel.mesh.vertices;

            Matrix4x4 matrix;
            Color32[] vertexColors;

            bool areAllCharactersFullAlpha = true;

            for (int i = 0; i < dialogueBoxLabel.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Skip this character if it is not visible
                if (!charInfo.isVisible)
                {
                    continue;
                }

                int vertexIndex = charInfo.vertexIndex;
                int materialIndex = charInfo.materialReferenceIndex;
                vertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Calculate the current alpha for each letter depending on whether it is fading/faded in or out
                float alpha = 0f;

                if (showText)
                {
                    if (typewriterInProgress && textAnimationData != null)
                    {
                        TextAnimationData animationData = textAnimationData[i];
                        float animationTime = Time.unscaledTime - typewriterStartTime - animationData.fadeInDelay;
                        alpha = Mathf.Clamp01(animationTime / animationData.fadeInDuration);
                    }
                    else
                    {
                        alpha = 1f;
                    }
                }
                else
                {
                    float animationTime = Time.unscaledTime - fadeOutStartTime;
                    alpha = 1f - Mathf.Clamp01(animationTime / fadeOutDuration);
                }

                // Keep track of whether all characters are fully visible for the sake of knowing when
                // the typewriter animation has finished.
                if (areAllCharactersFullAlpha && !Mathf.Approximately(alpha, 1f))
                {
                    areAllCharactersFullAlpha = false;
                }

                // Set the character's alpha
                Color32 color = charInfo.color;
                color.a = (byte)(255 * alpha);

                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;

                // Animate vertex positions
                if (textAnimationData != null && textAnimationData[i].animationStyle != null)
                {
                    TextAnimationData animationData = textAnimationData[i];
                    DialogueManager.TextAnimationStyle style = animationData.animationStyle;

                    // We want character animations to pivot at the character's center, so offset
                    // the vertices before we do any transformations
                    Vector3 offset = (charInfo.topLeft + charInfo.bottomRight) / 2;

                    vertices[vertexIndex + 0] -= offset;
                    vertices[vertexIndex + 1] -= offset;
                    vertices[vertexIndex + 2] -= offset;
                    vertices[vertexIndex + 3] -= offset;

                    // Calculate the animated translation, rotation and scale of the character
                    Vector3 translation = Vector3.zero;
                    if (style.translation.type != DialogueManager.TextAnimationStyle.Type.None)
                    {
                        float animationFactor = Mathf.Sin((Time.time - style.translation.delayPerCharacter * i) * Mathf.PI / style.translation.duration);
                        if (style.translation.type == DialogueManager.TextAnimationStyle.Type.Bounce)
                        {
                            animationFactor = Mathf.Abs(animationFactor);
                        }
                        translation = style.translation.offset * animationFactor;
                    }

                    Quaternion rotation = Quaternion.identity;
                    if (style.rotation.type != DialogueManager.TextAnimationStyle.Type.None)
                    {
                        float animationFactor = Mathf.Sin((Time.time - style.rotation.delayPerCharacter * i) * Mathf.PI / style.rotation.duration);
                        if (style.rotation.type == DialogueManager.TextAnimationStyle.Type.Bounce)
                        {
                            animationFactor = Mathf.Abs(animationFactor);
                        }
                        rotation = Quaternion.Euler(0f, 0f, style.rotation.startAngle + style.rotation.angleOffset * animationFactor);
                    }

                    Vector3 scale = Vector3.one;
                    if (style.scale.type != DialogueManager.TextAnimationStyle.Type.None)
                    {
                        float animationFactor = Mathf.Sin((Time.time - style.scale.delayPerCharacter * i) * Mathf.PI / style.scale.duration);
                        if (style.scale.type == DialogueManager.TextAnimationStyle.Type.Bounce)
                        {
                            animationFactor = Mathf.Abs(animationFactor);
                        }
                        scale = style.scale.startScale + style.scale.scaleOffset * animationFactor;
                        scale.z = 1f;
                    }

                    // Apply the calculated transformations
                    matrix = Matrix4x4.TRS(translation, rotation, scale);

                    vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                    vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                    // Undo the offset to put the character back in the right place
                    vertices[vertexIndex + 0] += offset;
                    vertices[vertexIndex + 1] += offset;
                    vertices[vertexIndex + 2] += offset;
                    vertices[vertexIndex + 3] += offset;
                }
            }

            // Mark the typewriter animation as over once all characters are fully visible
            if (typewriterInProgress && areAllCharactersFullAlpha)
            {
                typewriterInProgress = false;
            }

            // Update the mesh
            dialogueBoxLabel.mesh.vertices = vertices;

            dialogueBoxLabel.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        public void HideText()
        {
            showText = false;
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