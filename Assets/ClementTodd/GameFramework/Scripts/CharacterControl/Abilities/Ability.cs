using UnityEngine;

namespace ClementTodd.CharacterControl
{
    [RequireComponent(typeof(CharacterControl))]
    public abstract class Ability : MonoBehaviour
    {
        private CharacterControl _character;
        public CharacterControl character
        {
            get
            {
                if (!_character)
                {
                    _character = GetComponent<CharacterControl>();
                }
                return _character;
            }
        }
    }
}