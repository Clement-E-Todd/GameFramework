using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearGravitySource : GravitySource
{
    public Vector3 gravity;

    public override Vector3 GetGravityAtPosition(Vector3 worldPosition)
    {
        return gravity;
    }
}
