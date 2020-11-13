using UnityEngine;

namespace ClementTodd
{
    public abstract class BehaviourState : MonoBehaviour
    {
        private Character _Character;
        protected Character Character
        {
            get
            {
                if (_Character == null)
                {
                    _Character = GetComponent<Character>();
                }
                return _Character;
            }
        }

        public bool IsActive
        {
            get
            {
                return Character.CurrentState == this;
            }
        }

        public abstract void OnStateEnter();
        public abstract void OnStateExit();
    }
}