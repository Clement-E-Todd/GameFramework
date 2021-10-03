using UnityEngine;

namespace ClementTodd.EventGraphs
{
    public class SetObjectActiveNode : EventGraphNode
    {
        public int objectIndex;
        public bool active;

        public override void Execute()
        {
            if (objectIndex >= 0 && objectIndex < EventGraphManager.Instance.ObjectReferences.Length)
            {
                EventGraphManager.Instance.ObjectReferences[objectIndex].SetActive(active);
            }
            else
            {
                Debug.LogWarningFormat("Object reference index {0} of of range.", objectIndex);
            }

            DataGraph.ExecuteNextNode();
        }
    }
}