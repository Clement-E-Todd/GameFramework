using XNodeEditor;

namespace ClementTodd.EventGraphs
{
    [CustomNodeGraphEditor(typeof(EventGraph))]
    public class EventGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.IsSubclassOf(typeof(EventGraphNode)))
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