namespace ClementTodd.EventGraphs
{
    public class BoolConditionNode : ConditionNode
    {
        public string boolKey;
        public Properties.Scope scope;

        protected override bool Compare()
        {
            return scope == Properties.Scope.Local ?
                DataGraph.localProperties.Get<bool>(boolKey) :
                EventGraphManager.Instance.globalProperties.Get<bool>(boolKey);
        }
    }
}