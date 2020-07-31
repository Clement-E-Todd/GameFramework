using UnityEngine;
using TMPro;

namespace ClementTodd_v0_0_1
{
    public class DialogueCanvas : MonoBehaviour
    {
        public TextMeshProUGUI dialogueBoxText;
        public TextMeshProUGUI[] optionText;

        public void SetText(string text)
        {
            dialogueBoxText.text = text;
        }

        public void ShowOptionsBox(bool show)
        {
            Debug.LogFormat("STUB: ShowOptionsBox(show = {0})", show);
        }

        public void SetOptionText(int index, string text)
        {
            optionText[index].text = text;
            optionText[index].gameObject.SetActive(true);
        }

        public void ClearAllOptions()
        {
            for (int i = 0; i < optionText.Length; i++)
            {
                optionText[i].gameObject.SetActive(false);
            }
        }
    }
}