using System.Collections;
using UnityEngine;

namespace ClementTodd.EventGraphs.Dialogue
{
    public class DialogueHideBoxNode : DialogueNode
    {
        public override void Execute()
        {
            DialogueManager.Instance.canvas.HideAll();
            DataGraph.ExecuteNextNode();
        }
    }
}