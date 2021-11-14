using ClementTodd.NodeEvents;
using UnityEngine.InputSystem;

namespace ClementTodd.NodeEvents.Dialogue
{
    public abstract class DialogueNode : NodeEventNode
    {
        public virtual void OnNavigate(InputAction.CallbackContext context) { }
        public virtual void OnSubmitPressed(InputAction.CallbackContext context) { }
        public virtual void OnCancelPressed(InputAction.CallbackContext context) { }
    }
}
