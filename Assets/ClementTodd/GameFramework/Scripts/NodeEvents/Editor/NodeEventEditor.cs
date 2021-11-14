using XNodeEditor;

namespace ClementTodd.NodeEvents
{
    [CustomNodeGraphEditor(typeof(NodeEvent))]
    public class NodeEventEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.IsSubclassOf(typeof(NodeEventNode)))
            {
                return base.GetNodeMenuName(type);
            }
            else
            {
                return null;
            }
        }
    }
}