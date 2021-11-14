using UnityEngine;

namespace ClementTodd.Environment.Gravity
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