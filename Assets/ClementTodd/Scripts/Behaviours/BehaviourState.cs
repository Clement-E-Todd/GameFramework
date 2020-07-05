using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public abstract class BehaviourState : MonoBehaviour
    {
        public abstract BehaviourData data { get; }
    }
}