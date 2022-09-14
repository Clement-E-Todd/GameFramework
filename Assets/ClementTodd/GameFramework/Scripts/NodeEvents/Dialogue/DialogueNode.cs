using UnityEngine;

namespace ClementTodd.NodeEvents.Dialogue
{
    public abstract class DialogueNode : NodeEventNode
    {
        public virtual void OnNavigate(Vector2 navigation) { }
        public virtual void OnSubmitPressed() { }
        public virtual void OnCancelPressed() { }
    }
}
