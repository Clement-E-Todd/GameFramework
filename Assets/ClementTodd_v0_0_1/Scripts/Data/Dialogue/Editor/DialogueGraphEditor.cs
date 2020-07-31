using XNodeEditor;

namespace ClementTodd_v0_0_1
{
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.IsSubclassOf(typeof(DialogueNode)))
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