using XNode;

namespace ClementTodd.NodeEvents
{
    public abstract class NodeEventNode : Node
    {
        private NodeEvent _DataGraph;
        public NodeEvent DataGraph
        {
            get
            {
                if (_DataGraph == null)
                {
                    _DataGraph = graph as NodeEvent;
                }
                return _DataGraph;
            }
        }

        [System.Serializable]
        public class Connection { }

        [Input] public Connection previous;
        [Output] public Connection next;

        public override object GetValue(NodePort port)
        {
            return "Connected nodes: " + port.ConnectionCount;
        }

        public abstract void Invoke();
    }
}