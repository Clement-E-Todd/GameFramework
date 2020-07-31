using UnityEngine;

namespace ClementTodd_v0_0_1
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