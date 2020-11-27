using System.Collections;
using UnityEngine;

namespace ClementTodd.GameFramework
{
    public class DialogueHideDialogueBoxNode : DialogueNode
    {
        public override void Execute()
        {
            DialogueManager.Instance.canvas.HideAll();
            DialogueGraph.ExecuteNextNode();
        }
    }
}