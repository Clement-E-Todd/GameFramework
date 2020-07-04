using UnityEngine;

namespace ClementTodd.Characters
{
    [RequireComponent(typeof(GravityReceiver))]
    public class MoveAbility : Ability
    {
        public float speed = 2f;
        public float runSpeed = 4f;

        public float turnSpeed = 360f;

        public float airFriction = 0.05f;

        public Transform groundAnchor;

        private Vector3 momentum = Vector3.zero;

        private GravityReceiver gravityReceiver;

        private const float minMove = 0.001f;

        private const float maxGroundDistance = 0.2f;


        private void Awake()
        {
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        private void FixedUpdate()
        {
            if (behaviourData.move.sqrMagnitude >= minMove || momentum.sqrMagnitude >= minMove)
            {
                // Calculate the variables needed to move based on the character's human-or-computer-controller behaviour
                Vector3 moveDirection = new Vector3(behaviourData.move.x, 0f, behaviourData.move.y);
                float speed = behaviourData.run ? runSpeed : this.speed;
                float maxAcceleration = speed / Time.fixedDeltaTime;
                float friction = gravityReceiver.isGrounded ? gravityReceiver.groundFriction : Mathf.Min(gravityReceiver.groundFriction, airFriction);
                momentum = Vector3.MoveTowards(momentum, moveDirection * speed, maxAcceleration * friction * Time.fixedDeltaTime);

                // If the character's momentum counteracts their Rigidbody's velocity, reduce the velocity.
                // Only works on the ground.
                if (gravityReceiver.isGrounded && !Mathf.Approximately(momentum.sqrMagnitude, 0f))
                {
                    Vector3 momentumDirection = momentum.normalized;
                    float opposingVelocityMagnitude = -Vector3.Dot(character.body.velocity, momentumDirection);

                    if (opposingVelocityMagnitude > 0f)
                    {
                        float force = Mathf.Min(momentum.magnitude * gravityReceiver.gripOnGround, opposingVelocityMagnitude);
                        character.body.AddForce(momentumDirection * force);
                    }
                }

                // Turn to face the direction that the character *intends* to move in.
                if (!Mathf.Approximately(moveDirection.sqrMagnitude, 0f))
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, transform.up);
                    character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
                }

                // Calculate the actual amount by which the character should move
                Vector3 movement = momentum;

                if (gravityReceiver && gravityReceiver.isGrounded)
                {
                    // Align movement to ground for better control
                    movement = gravityReceiver.AlignVectorToGround(movement);

                    // Push lightly into the ground to avoid awkward hops when the slope angle changes
                    if (groundAnchor)
                    {
                        RaycastHit hit;
                        bool overGround = Physics.Raycast(groundAnchor.position, gravityReceiver.gravityDirection, out hit, maxGroundDistance);
                        if (overGround)
                        {
                            movement -= gravityReceiver.groundNormal * hit.distance / Time.fixedDeltaTime;
                        }
                    }
                }

                // Move the character by the calculated amount
                character.body.MovePosition(character.transform.position + movement * Time.fixedDeltaTime);
            }
        }
    }
}