using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GravitySource : MonoBehaviour
{
    public abstract Vector3 GetGravityAtPosition(Vector3 worldPosition);

    private void OnEnable()
    {
        GravityManager.instance.AddSource(this);
    }

    private void OnDisable()
    {
        GravityManager.instance.RemoveSource(this);
    }
}
