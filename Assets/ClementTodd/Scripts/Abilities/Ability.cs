using UnityEngine;

namespace ClementTodd
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
    }
}