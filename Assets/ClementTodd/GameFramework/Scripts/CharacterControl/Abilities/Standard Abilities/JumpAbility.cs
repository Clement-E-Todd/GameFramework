using ClementTodd.Environment.Gravity;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GravityReceiver))]
    public class JumpAbility : Ability
    {
        public float jumpForce = 500f;

        public float maxHoldTime = 0.125f;

        private Rigidbody body;

        private GravityReceiver gravityReceiver;

        private float jumpStartTime = 0f;

        private bool jumpHeld = false;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        public void StartJump()
        {
            if (!jumpHeld && gravityReceiver.isGrounded)
            {
                gravityReceiver.LaunchUpwards(jumpForce);
                jumpStartTime = Time.time;
                jumpHeld = true;
            }
        }

        public void EndJump()
        {
            jumpHeld = false;
        }

        private void FixedUpdate()
        {
            // Sustain the jump as long as the jump control is held
            if (jumpHeld && !gravityReceiver.isGrounded && Time.time <= jumpStartTime + maxHoldTime)
            {
                body.AddForce(-gravityReceiver.gravity * gravityReceiver.gravityScale);
            }
        }
    }
}