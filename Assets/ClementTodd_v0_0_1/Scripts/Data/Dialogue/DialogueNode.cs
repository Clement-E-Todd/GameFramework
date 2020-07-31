using XNode;
using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
    // Nodes must derive from DialogueNode in order to be added to a DialogueGraph.
    public abstract class DialogueNode : Node
    {
        private DialogueGraph _DialogueGraph;
        public DialogueGraph DialogueGraph
        {
            get
            {
                if (_DialogueGraph == null)
                {
                    _DialogueGraph = graph as DialogueGraph;
                }
                return _DialogueGraph;
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

        public virtual void OnNavigate(InputAction.CallbackContext context) { }
        public virtual void OnSubmitPressed(InputAction.CallbackContext context) { }
        public virtual void OnCancelPressed(InputAction.CallbackContext context) { }
    }
}