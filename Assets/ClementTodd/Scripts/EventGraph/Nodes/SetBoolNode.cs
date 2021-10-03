namespace ClementTodd.EventGraphs
{
    public class SetBoolNode : EventGraphNode
    {
        public string key;
        public bool value;

        public Properties.Scope scope;

        public override void Execute()
        {
            switch (scope)
            {
                case Properties.Scope.Local:
                    DataGraph.localProperties.Set(key, value);
                    break;

                case Properties.Scope.Global:
                    EventGraphManager.Instance.globalProperties.Set(key, value);
                    break;
            }

            DataGraph.ExecuteNextNode();
        }
    }
}