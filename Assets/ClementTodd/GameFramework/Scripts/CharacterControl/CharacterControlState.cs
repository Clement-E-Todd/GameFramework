using UnityEngine;

namespace ClementTodd.CharacterControl
{
    public abstract class CharacterControlState : MonoBehaviour
    {
        private CharacterControl _Character;
        protected CharacterControl Character
        {
            get
            {
                if (_Character == null)
                {
                    _Character = GetComponent<CharacterControl>();
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