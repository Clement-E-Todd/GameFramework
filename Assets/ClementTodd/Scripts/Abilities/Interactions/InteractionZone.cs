using UnityEngine;

namespace ClementTodd
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractionZone : MonoBehaviour
    {
        public Transform sourceTransform;

        public Transform uiTargetTransform;

        public bool callerMustFaceSource = true;

        public TextAsset textData;
        public string actionNameTextKey;
        public string prepositionTextKey;

        public abstract void DoInteraction(InteractAbility caller);

        public bool CanInteractionBeCalled(Transform callerTransform)
        {
            return !callerMustFaceSource || (sourceTransform && Vector3.Dot(callerTransform.forward, sourceTransform.position - callerTransform.transform.position) > 0f);
        }

        public void OnTargetedByPlayer()
        {
            string actionName = LocalizationManager.Instance.GetString(textData, actionNameTextKey);
            string preposition = LocalizationManager.Instance.GetString(textData, prepositionTextKey);

            HUD.instance.ShowInteractionPrompt(actionName, preposition + " " + sourceTransform.name);
        }
    }
}