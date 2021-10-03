using ClementTodd.EventGraphs;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    public class DialogueInteraction : InteractionZone
    {
        [Header("Dialogue Interaction Properties")]
        public EventGraph dialogue;
        public GameObject[] objectReferences;

        public override void DoInteraction(InteractAbility caller)
        {
            EventGraphManager.Instance.StartEvent(dialogue, objectReferences);
        }
    }
}