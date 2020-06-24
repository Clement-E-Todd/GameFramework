using UnityEngine;

namespace ClementTodd.Characters
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
                character.body.AddForce(-gravityReceiver.gravityDirection * jumpForce);
                jumpStartTime = Time.time;
            }
            else if (jumpHeld && Time.time <= jumpStartTime + maxHoldTime)
            {
                Vector3 gravity = GravityManager.instance.GetGravityAtPosition(body.centerOfMass);
                body.AddForce(-gravity * gravityReceiver.gravityScale);
            }
        }
    }
}