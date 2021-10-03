namespace ClementTodd.EventGraphs
{
    public abstract class ConditionNode : EventGraphNode
    {
        [Output] public Connection elseNext;

        public override void Execute()
        {
            if (Compare())
            {
                DataGraph.ExecuteNextNode();
            }
            else
            {
                DataGraph.ExecuteNextNode(0, "elseNext");
            }
        }

        protected abstract bool Compare();
    }
}