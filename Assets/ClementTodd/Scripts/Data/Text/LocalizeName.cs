using UnityEngine;

namespace ClementTodd_v0_0_1
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