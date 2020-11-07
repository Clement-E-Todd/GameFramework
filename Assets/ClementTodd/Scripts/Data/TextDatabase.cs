using UnityEngine;

namespace ClementTodd_v0_0_1
{
    [CreateAssetMenu(fileName = "TextDatabase", menuName = "Clement Todd/Data/Text Database", order = 0)]
    public class TextDatabase : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {


            [System.Serializable]
            public class Language
            {
                public string text;
            }

            public Language english;
            public Language test;
        }
    }
}