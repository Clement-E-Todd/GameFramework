using ClementTodd.DataManagement;

namespace ClementTodd.NodeEvents
{
    public class SetIntNode : NodeEventNode
    {
        public string key;
        public int value;

        public Properties.Scope scope;

        public bool relative;

        public override void Invoke()
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
                        int oldValue = NodeEventManager.Instance.globalProperties.Get<int>(key);
                        NodeEventManager.Instance.globalProperties.Set(key, oldValue + value);
                    }
                    else
                    {
                        NodeEventManager.Instance.globalProperties.Set(key, value);
                    }
                    break;
            }

            DataGraph.ExecuteNextNode();
        }
    }
}