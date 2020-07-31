using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class GravityManager : MonoBehaviour
    {
        private static GravityManager _instance;
        public static GravityManager instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<GravityManager>();

                    if (!_instance)
                    {
                        _instance = new GameObject("Gravity Manager", typeof(GravityManager)).GetComponent<GravityManager>();
                    }
                }

                return _instance;
            }
        }

        private List<GravitySource> sources = new List<GravitySource>();

        private void OnEnable()
        {
            if (!Mathf.Approximately(Physics.gravity.sqrMagnitude, 0f))
            {
                Debug.LogWarning("Physics.gravity is overridden by GravityManager. It will be set to (0,0,0).");
                Physics.gravity = Vector3.zero;
            }
        }

        public void AddSource(GravitySource source)
        {
            sources.Add(source);
        }

        public void RemoveSource(GravitySource source)
        {
            sources.Remove(source);
        }

        public Vector3 GetGravityAtPosition(Vector3 worldPosition)
        {
            Vector3 totalGravity = Vector3.zero;

            for (int i = 0; i < sources.Count; i++)
            {
                totalGravity += sources[i].GetGravityAtPosition(worldPosition);
            }

            return totalGravity;
        }
    }
}