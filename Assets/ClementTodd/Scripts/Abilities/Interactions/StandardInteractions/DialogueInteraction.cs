using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class DialogueInteraction : InteractionZone
    {
        public override void DoInteraction(InteractAbility caller)
        {
            Debug.LogFormat("{0} interacted with {1} at timestamp {2}", caller.name, sourceTransform.name, Time.time);
        }
    }
}