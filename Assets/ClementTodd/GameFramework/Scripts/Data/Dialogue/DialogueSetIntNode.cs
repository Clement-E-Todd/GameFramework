namespace ClementTodd.GameFramework
{
    public class DialogueSetIntNode : DialogueNode
    {
        public string key;
        public int value;

        public DialoguePropertyScope scope;

        public bool relative;

        public override void Execute()
        {
            switch (scope)
            {
                case DialoguePropertyScope.Local:
                    if (relative)
                    {
                        int oldValue = DialogueGraph.localProperties.Get<int>(key);
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
                        int oldValue = DialogueManager.Instance.globalProperties.Get<int>(key);
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