using System.Collections;
using UnityEngine;

namespace ClementTodd.GameFramework
{
    public class DialogueWaitNode : DialogueNode
    {
        public float duration;
        public bool unscaled;

        public override void Execute()
        {
            DialogueManager.Instance.StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            if (unscaled)
            {
                yield return new WaitForSecondsRealtime(duration);
            }
            else
            {
                yield return new WaitForSeconds(duration);
            }

            DialogueGraph.ExecuteNextNode();
        }
    }
}