using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class LinearGravitySource : GravitySource
    {
        public Vector3 gravity;

        public override Vector3 GetGravityAtPosition(Vector3 worldPosition)
        {
            return gravity;
        }
    }
}