using ClementTodd.DataManagement;

namespace ClementTodd.NodeEvents
{
    public class SetFloatNode : NodeEventNode
    {
        public string key;
        public float value;

        public Properties.Scope scope;

        public bool relative;

        public override void Invoke()
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
                        float oldValue = NodeEventManager.Instance.globalProperties.Get<float>(key);
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