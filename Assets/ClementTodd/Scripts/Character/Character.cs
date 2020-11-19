using UnityEngine;

namespace ClementTodd
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour
    {
        public Rigidbody body { get; private set; }

        public CharacterControlState defaultControlState;
        public CharacterControlState CurrentControlState { get; private set; }

        public System.Action<CharacterControlState> OnControlStateEnter;
        public System.Action<CharacterControlState> OnControlStateExit;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // Init control state
            if (defaultControlState == null)
            {
                defaultControlState = GetComponent<CharacterControlState>();
            }
            SetControlState(defaultControlState);
        }

        protected void SetControlState(CharacterControlState state)
        {
            if (CurrentControlState)
            {
                CurrentControlState.OnStateExit();
                OnControlStateExit?.Invoke(CurrentControlState);
            }

            CurrentControlState = state;

            if (CurrentControlState)
            {
                CurrentControlState.OnStateEnter();
                OnControlStateEnter?.Invoke(CurrentControlState);
            }
        }
    }
}