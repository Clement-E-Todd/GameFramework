using ClementTodd.Localization;
using UnityEngine.InputSystem;

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

        public override void OnSubmitPressed(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.canvas.optionMenu.OnConfirm(context);
        }

        public override void OnNavigate(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.canvas.optionMenu.OnNavigate(context);
        }
    }
}