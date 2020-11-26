namespace ClementTodd.GameFramework
{
    public class DialogueSetBoolNode : DialogueNode
    {
        public string key;
        public bool value;

        public DialoguePropertyScope scope;

        public override void Execute()
        {
            switch (scope)
            {
                case DialoguePropertyScope.Local:
                    DialogueGraph.localProperties.Set(key, value);
                    break;

                case DialoguePropertyScope.Global:
                    DialogueManager.Instance.globalProperties.Set(key, value);
                    break;
            }

            DialogueGraph.ExecuteNextNode();
        }
    }
}