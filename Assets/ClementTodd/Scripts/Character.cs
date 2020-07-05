using UnityEngine;

namespace ClementTodd_v0_0_1
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        public Rigidbody body { get; private set; }

        public BehaviourState defaultState;
        public BehaviourState currentState { get; private set; }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();

            if (defaultState == null)
            {
                defaultState = GetComponent<BehaviourState>();
            }
            currentState = defaultState;
        }
    }
}