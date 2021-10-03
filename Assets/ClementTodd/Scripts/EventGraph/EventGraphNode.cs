using XNode;

namespace ClementTodd.EventGraphs
{
    public abstract class EventGraphNode : Node
    {
        private EventGraph _DataGraph;
        public EventGraph DataGraph
        {
            get
            {
                if (_DataGraph == null)
                {
                    _DataGraph = graph as EventGraph;
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

        public abstract void Execute();
    }
}