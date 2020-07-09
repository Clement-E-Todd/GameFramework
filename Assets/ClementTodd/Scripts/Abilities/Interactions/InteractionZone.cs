using UnityEngine;

namespace ClementTodd_v0_0_1
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractionZone : MonoBehaviour
    {
        public Transform sourceTransform;

        public Transform uiTargetTransform;

        public bool callerMustFaceSource = true;

        public abstract string ActionName { get; }
        public abstract string Preposition { get; }

        public string overrideSourceName;

        public abstract void DoInteraction(InteractAbility caller);

        public bool CanInteractionBeCalled(Transform callerTransform)
        {
            return !callerMustFaceSource || (sourceTransform && Vector3.Dot(callerTransform.forward, sourceTransform.position - callerTransform.transform.position) > 0f);
        }

        public void OnTargetedByPlayer(InteractAbility callerInteractAbility)
        {
            HUD.instance.ShowInteractionPrompt(ActionName, Preposition + " " + (overrideSourceName.Length > 0 ? overrideSourceName : sourceTransform.name));
        }


    }
}