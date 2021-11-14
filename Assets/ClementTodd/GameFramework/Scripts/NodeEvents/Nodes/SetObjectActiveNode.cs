using UnityEngine;

namespace ClementTodd.NodeEvents
{
    public class SetObjectActiveNode : NodeEventNode
    {
        public int objectIndex;
        public bool active;

        public override void Invoke()
        {
            if (objectIndex >= 0 && objectIndex < NodeEventManager.Instance.ObjectReferences.Length)
            {
                NodeEventManager.Instance.ObjectReferences[objectIndex].SetActive(active);
            }
            else
            {
                Debug.LogWarningFormat("Object reference index {0} of of range.", objectIndex);
            }

            DataGraph.ExecuteNextNode();
        }
    }
}