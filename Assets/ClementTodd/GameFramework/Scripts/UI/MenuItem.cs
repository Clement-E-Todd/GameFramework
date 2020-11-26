using UnityEngine;
using UnityEngine.Events;

namespace ClementTodd.GameFramework
{
    public class MenuItem : MonoBehaviour
    {
        public UnityEvent onSelected;

        [System.Serializable]
        public class Neighbours
        {
            public MenuItem up;
            public MenuItem down;
            public MenuItem left;
            public MenuItem right;
        }
        public Neighbours neighbours;

        private RectTransform _RectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_RectTransform == null)
                {
                    _RectTransform = transform as RectTransform;
                }
                return _RectTransform;
            }
        }
    }
}