using ClementTodd.Localization;
using UnityEngine;

namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueOptionNode : DialogueNode
    {
        public string[] optionKeys;

        public override void Invoke()
        {
            DialogueManager.Instance.canvas.ClearAllOptions();

            for (int i = 0; i < optionKeys.Length; i++)
            {
                string text = LocalizationManager.Instance.GetString(DataGraph.textData, optionKeys[i]);
                DialogueManager.Instance.canvas.SetOptionText(i, text);
            }
            DialogueManager.Instance.canvas.UpdateOptionLinks();

            DialogueManager.Instance.canvas.ShowOptionsBox();
        }

        public override void OnSubmitPressed()
        {
            DialogueManager.Instance.canvas.optionMenu.OnConfirm();
        }

        public override void OnNavigate(Vector2 navigation)
        {
            DialogueManager.Instance.canvas.optionMenu.OnNavigate(navigation);
        }
    }
}