using UnityEngine.InputSystem;

namespace ClementTodd.EventGraphs.Dialogue
{
	public abstract class DialogueNode : EventGraphNode
	{
		public virtual void OnNavigate(InputAction.CallbackContext context) { }
		public virtual void OnSubmitPressed(InputAction.CallbackContext context) { }
		public virtual void OnCancelPressed(InputAction.CallbackContext context) { }
	}
}
