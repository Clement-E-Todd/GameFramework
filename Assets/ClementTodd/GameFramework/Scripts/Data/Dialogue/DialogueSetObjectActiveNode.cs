using UnityEngine;

namespace ClementTodd.GameFramework
{
    public class DialogueSetObjectActiveNode : DialogueNode
    {
        public int objectIndex;
        public bool active;

        public override void Execute()
        {
            if (objectIndex >= 0 && objectIndex < DialogueManager.Instance.ObjectReferences.Length)
            {
                DialogueManager.Instance.ObjectReferences[objectIndex].SetActive(active);
            }
            else
            {
                Debug.LogWarningFormat("Object reference index {0} of of range.", objectIndex);
            }

            DialogueGraph.ExecuteNextNode();
        }
    }
}