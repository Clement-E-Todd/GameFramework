using UnityEngine;

namespace ClementTodd.GameFramework
{
    public abstract class CharacterControlState : MonoBehaviour
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
                return Character.CurrentControlState == this;
            }
        }

        public abstract void OnStateEnter();
        public abstract void OnStateExit();
    }
}