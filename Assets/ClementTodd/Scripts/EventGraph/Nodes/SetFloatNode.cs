namespace ClementTodd.EventGraphs
{
    public class SetFloatNode : EventGraphNode
    {
        public string key;
        public float value;

        public Properties.Scope scope;

        public bool relative;

        public override void Execute()
        {
            switch (scope)
            {
                case Properties.Scope.Local:
                    if (relative)
                    {
                        float oldValue = DataGraph.localProperties.Get<float>(key);
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
                        float oldValue = EventGraphManager.Instance.globalProperties.Get<float>(key);
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