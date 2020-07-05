using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public abstract class GravitySource : MonoBehaviour
    {
        public abstract Vector3 GetGravityAtPosition(Vector3 worldPosition);

        private void OnEnable()
        {
            GravityManager.instance.AddSource(this);
        }

        private void OnDisable()
        {
            GravityManager.instance.RemoveSource(this);
        }
    }
}