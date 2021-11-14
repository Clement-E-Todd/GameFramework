namespace ClementTodd.NodeEvents
{
    public abstract class ConditionNode : NodeEventNode
    {
        [Output] public Connection elseNext;

        public override void Invoke()
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