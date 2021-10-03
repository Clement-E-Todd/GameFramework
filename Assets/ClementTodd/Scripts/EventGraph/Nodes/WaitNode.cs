using System.Collections;
using UnityEngine;

namespace ClementTodd.EventGraphs
{
    public class WaitNode : EventGraphNode
    {
        public float duration;
        public bool unscaled;

        public override void Execute()
        {
            EventGraphManager.Instance.StartCoroutine(Wait());
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

            DataGraph.ExecuteNextNode();
        }
    }
}