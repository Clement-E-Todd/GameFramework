using UnityEngine;

namespace ClementTodd.GameFramework
{
    public class DialogueInteraction : InteractionZone
    {
        [Header("Dialogue Interaction Properties")]
        public DialogueGraph dialogue;

        public override void DoInteraction(InteractAbility caller)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}