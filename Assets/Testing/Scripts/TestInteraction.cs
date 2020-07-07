﻿using ClementTodd_v0_0_1;
using UnityEngine;

public class TestInteraction : InteractionZone
{
    public override void DoInteraction(InteractAbility caller)
    {
        Debug.LogFormat("{0} interacted with {1} at timestamp {2}", caller.name, sourceTransform.name, Time.time);
    }
}