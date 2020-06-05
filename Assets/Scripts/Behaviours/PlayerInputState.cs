using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd.Characters
{
    public class PlayerInputState : BehaviourState
    {
        public override BehaviourData data
        {
            get
            {
                BehaviourData data = new BehaviourData();

                data.moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                data.run = Input.GetKey(KeyCode.LeftShift);
                data.jump = Input.GetKey(KeyCode.Space);

                return data;
            }
        }
    }
}