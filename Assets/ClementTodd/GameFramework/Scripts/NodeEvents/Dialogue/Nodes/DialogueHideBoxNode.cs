namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueHideBoxNode : DialogueNode
    {
        public override void Invoke()
        {
            DialogueManager.Instance.canvas.HideAll();
            DataGraph.ExecuteNextNode();
        }
    }
}