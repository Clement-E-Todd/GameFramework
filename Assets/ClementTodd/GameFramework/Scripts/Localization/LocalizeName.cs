using UnityEngine;

namespace ClementTodd.Localization
{
    public class LocalizeName : MonoBehaviour
    {
        public TextAsset data;
        public string key;

        void Start()
        {
            name = LocalizationManager.Instance.GetString(data, key);
        }
    }
}