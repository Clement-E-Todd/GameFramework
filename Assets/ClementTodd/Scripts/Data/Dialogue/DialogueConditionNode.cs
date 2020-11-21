namespace ClementTodd
{
    public abstract class DialogueConditionNode : DialogueNode
    {
        [Output] public Connection elseNext;

        public override void Execute()
        {
            if (Compare())
            {
                DialogueGraph.ExecuteNextNode();
            }
            else
            {
                DialogueGraph.ExecuteNextNode(0, "elseNext");
            }
        }

        protected abstract bool Compare();
    }
}