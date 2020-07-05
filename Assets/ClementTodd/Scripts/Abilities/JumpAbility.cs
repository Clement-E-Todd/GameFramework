using UnityEngine;

namespace ClementTodd_v0_0_1
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

        private void FixedUpdate()
        {
            bool jumpTapped = behaviourData.jump && !jumpHeld;
            jumpHeld = behaviourData.jump;

            if (jumpTapped && gravityReceiver.isGrounded)
            {
                gravityReceiver.LaunchUpwards(jumpForce);
                jumpStartTime = Time.time;
            }
            else if (jumpHeld && !gravityReceiver.isGrounded && Time.time <= jumpStartTime + maxHoldTime)
            {
                body.AddForce(-gravityReceiver.gravity * gravityReceiver.gravityScale);
            }
        }
    }
}