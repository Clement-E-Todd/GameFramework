using UnityEngine;

namespace ClementTodd_v0_0_1
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractionZone : MonoBehaviour
    {
        public Transform sourceTransform;

        public bool callerMustFaceSource = true;

        public abstract void DoInteraction(InteractAbility caller);

        public bool CanInteractionBeCalled(Transform callerTransform)
        {
            return !callerMustFaceSource || (sourceTransform && Vector3.Dot(callerTransform.forward, sourceTransform.position - callerTransform.transform.position) > 0f);
        }
    }
}