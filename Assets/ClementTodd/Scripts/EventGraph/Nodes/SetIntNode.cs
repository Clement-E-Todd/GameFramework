namespace ClementTodd.EventGraphs
{
    public class SetIntNode : EventGraphNode
    {
        public string key;
        public int value;

        public Properties.Scope scope;

        public bool relative;

        public override void Execute()
        {
            switch (scope)
            {
                case Properties.Scope.Local:
                    if (relative)
                    {
                        int oldValue = DataGraph.localProperties.Get<int>(key);
                        DataGraph.localProperties.Set(key, oldValue + value);
                    }
                    else
                    {
                        DataGraph.localProperties.Set(key, value);
                    }
                    break;

                case Properties.Scope.Global:
                    if (relative)
                    {
                        int oldValue = EventGraphManager.Instance.globalProperties.Get<int>(key);
                        EventGraphManager.Instance.globalProperties.Set(key, oldValue + value);
                    }
                    else
                    {
                        EventGraphManager.Instance.globalProperties.Set(key, value);
                    }
                    break;
            }

            DataGraph.ExecuteNextNode();
        }
    }
}