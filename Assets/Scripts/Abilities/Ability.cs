using UnityEngine;

namespace ClementTodd.Characters
{
    [RequireComponent(typeof(Character))]
    public abstract class Ability : MonoBehaviour
    {
        private Character _character;
        public Character character
        {
            get
            {
                if (!_character)
                {
                    _character = GetComponent<Character>();
                }
                return _character;
            }
        }

        protected BehaviourData behaviourData
        {
            get
            {
                return character.currentState ? character.currentState.data : null;
            }
        }
    }
}