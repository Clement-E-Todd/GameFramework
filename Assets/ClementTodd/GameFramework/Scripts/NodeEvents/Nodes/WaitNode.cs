using System.Collections;
using UnityEngine;

namespace ClementTodd.NodeEvents
{
    public class WaitNode : NodeEventNode
    {
        public float duration;
        public bool unscaled;

        public override void Invoke()
        {
            NodeEventManager.Instance.StartCoroutine(Wait());
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