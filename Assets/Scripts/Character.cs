using UnityEngine;

namespace ClementTodd.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        public CharacterController controller { get; private set; }

        public BehaviourState defaultState;
        public BehaviourState currentState { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            if (defaultState == null)
            {
                defaultState = GetComponent<BehaviourState>();
            }
            currentState = defaultState;
        }
    }
}