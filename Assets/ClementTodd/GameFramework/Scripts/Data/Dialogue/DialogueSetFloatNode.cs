namespace ClementTodd.GameFramework
{
    public class DialogueSetFloatNode : DialogueNode
    {
        public string key;
        public float value;

        public DialoguePropertyScope scope;

        public bool relative;

        public override void Execute()
        {
            switch (scope)
            {
                case DialoguePropertyScope.Local:
                    if (relative)
                    {
                        float oldValue = DialogueGraph.localProperties.Get<float>(key);
                        DialogueGraph.localProperties.Set(key, oldValue + value);
                    }
                    else
                    {
                        DialogueGraph.localProperties.Set(key, value);
                    }
                    break;

                case DialoguePropertyScope.Global:
                    if (relative)
                    {
                        float oldValue = DialogueManager.Instance.globalProperties.Get<float>(key);
                        DialogueManager.Instance.globalProperties.Set(key, oldValue + value);
                    }
                    else
                    {
                        DialogueManager.Instance.globalProperties.Set(key, value);
                    }
                    break;
            }

            DialogueGraph.ExecuteNextNode();
        }
    }
}