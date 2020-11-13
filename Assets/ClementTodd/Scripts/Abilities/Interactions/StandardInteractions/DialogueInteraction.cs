using UnityEngine;

namespace ClementTodd
{
    public class DialogueInteraction : InteractionZone
    {
        public DialogueGraph dialogue;

        public override void DoInteraction(InteractAbility caller)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}