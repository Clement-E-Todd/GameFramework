namespace ClementTodd
{
    public class DialogueBoolConditionNode : DialogueConditionNode
    {
        public string boolKey;
        public DialoguePropertyScope scope;

        protected override bool Compare()
        {
            return scope == DialoguePropertyScope.Local ?
                DialogueGraph.localProperties.Get<bool>(boolKey) :
                DialogueManager.Instance.globalProperties.Get<bool>(boolKey);
        }
    }
}