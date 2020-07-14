using XNode;

namespace ClementTodd_v0_0_1
{
    // Nodes must derive from DialogueNode in order to be added to a DialogueGraph.
    public abstract class DialogueNode : Node
    {
        [System.Serializable]
        public class Connection { }

        [Input] public Connection previous;
        [Output] public Connection next;

        public override object GetValue(NodePort port)
        {
            return "Connected nodes: " + port.ConnectionCount;
        }
    }
}