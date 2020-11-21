using UnityEngine;
using XNode;

namespace ClementTodd
{
    [CreateAssetMenu(
        fileName = "New Dialogue Graph",
        menuName = nameof(ClementTodd) + "/Dialogue Graph",
        order = 0)]

    public class DialogueGraph : NodeGraph
    {
        public bool IsActive
        {
            get
            {
                return DialogueManager.Instance.CurrentDialogue == this;
            }
        }

        public DialogueNode CurrentNode { get; private set; }

        public DialogueNode startNode;

        public TextAsset textData;

        public Properties localProperties = new Properties();

        public void ExecuteStartNode()
        {
            if (IsActive)
            {
                CurrentNode = startNode;
                CurrentNode.Execute();
            }
            else
            {
                Debug.LogWarning("Can't execute node: this dialogue is not active.");
            }
        }

        public void ExecuteNextNode(int connectionIndex = 0, string nodeName = "next")
        {
            if (IsActive)
            {
                NodePort port = CurrentNode.GetOutputPort(nodeName);

                if (connectionIndex >= 0 && connectionIndex < port.ConnectionCount)
                {
                    CurrentNode = port.GetConnection(connectionIndex).node as DialogueNode;
                    CurrentNode.Execute();
                }
                else
                {
                    DialogueManager.Instance.EndDialogue();
                }
            }
            else
            {
                Debug.LogWarning("Can't execute node: this dialogue is not active.");
            }
        }
    }
}