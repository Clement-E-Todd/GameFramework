using UnityEngine;

namespace ClementTodd.Characters
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