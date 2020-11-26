using UnityEngine;

namespace ClementTodd.GameFramework
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractionZone : MonoBehaviour
    {
        [Header("Base Interaction Properties")]
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

        public string GetTargetName()
        {
            return sourceTransform.name;
        }

        public string GetActionName()
        {
            return LocalizationManager.Instance.GetString(textData, actionNameTextKey);
        }

        public string GetPreposition()
        {
            return LocalizationManager.Instance.GetString(textData, prepositionTextKey);
        }
    }
}