using ClementTodd.DataManagement;

namespace ClementTodd.NodeEvents
{
    public class SetBoolNode : NodeEventNode
    {
        public string key;
        public bool value;

        public Properties.Scope scope;

        public override void Invoke()
        {
            switch (scope)
            {
                case Properties.Scope.Local:
                    DataGraph.localProperties.Set(key, value);
                    break;

                case Properties.Scope.Global:
                    NodeEventManager.Instance.globalProperties.Set(key, value);
                    break;
            }

            DataGraph.ExecuteNextNode();
        }
    }
}