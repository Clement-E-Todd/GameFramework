using UnityEngine;

namespace ClementTodd.Characters
{
    public abstract class BehaviourState : MonoBehaviour
    {
        public abstract BehaviourData data { get; }
    }
}