using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class DialogueInteraction : InteractionZone
    {
        public DialogueGraph dialogue;
        public DialogueNode defaultStartNode;

        public override void DoInteraction(InteractAbility caller)
        {
            DialogueManager.Instance.StartDialogue(dialogue, defaultStartNode);
        }
    }
}