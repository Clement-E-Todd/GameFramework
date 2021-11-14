using ClementTodd.Localization;
using UnityEngine;

namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueSetNameNode : DialogueNode
    {
        public string key;
        public TextAsset overrideData;

        public override void Invoke()
        {
            string name = LocalizationManager.Instance.GetString(overrideData ? overrideData : DataGraph.textData, key);
            DialogueManager.Instance.canvas.SetName(name);
            DataGraph.ExecuteNextNode();
        }
    }
}