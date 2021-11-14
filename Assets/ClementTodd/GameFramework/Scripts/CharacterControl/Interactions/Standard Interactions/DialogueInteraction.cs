using ClementTodd.NodeEvents;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    public class DialogueInteraction : InteractionZone
    {
        [Header("Dialogue Interaction Properties")]
        public NodeEvent dialogue;
        public GameObject[] objectReferences;

        public override void DoInteraction(InteractAbility caller)
        {
            NodeEventManager.Instance.StartEvent(dialogue, objectReferences);
        }
    }
}