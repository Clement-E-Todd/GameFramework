using UnityEngine;

namespace ClementTodd.GameFramework
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