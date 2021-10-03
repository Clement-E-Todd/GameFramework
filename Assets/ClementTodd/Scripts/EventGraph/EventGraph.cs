using UnityEngine;
using XNode;

namespace ClementTodd.EventGraphs
{
    [CreateAssetMenu(fileName = "New DataGraph", menuName = nameof(ClementTodd) + "/DataGraph", order = 1)]
    public class EventGraph : NodeGraph
    {
        public bool IsActive
        {
            get
            {
                return EventGraphManager.Instance.CurrentEvent == this;
            }
        }

        public EventGraphNode CurrentNode { get; private set; }

        public EventGraphNode startNode;

        public TextAsset textData;

        public Properties localProperties = new Properties();

        public void ExecuteStartNode()
        {
            if (IsActive)
            {
                CurrentNode = startNode;
                CurrentNode.Execute();
            }
            else
            {
                Debug.LogWarning("Can't execute node: this event is not active.");
            }
        }

        public void ExecuteNextNode(int connectionIndex = 0, string nodeName = "next")
        {
            if (IsActive)
            {
                NodePort port = CurrentNode.GetOutputPort(nodeName);

                if (connectionIndex >= 0 && connectionIndex < port.ConnectionCount)
                {
                    CurrentNode = port.GetConnection(connectionIndex).node as EventGraphNode;
                    CurrentNode.Execute();
                }
                else
                {
                    EventGraphManager.Instance.EndEvent();
                }
            }
            else
            {
                Debug.LogWarning("Can't execute node: this event is not active.");
            }
        }
    }
}