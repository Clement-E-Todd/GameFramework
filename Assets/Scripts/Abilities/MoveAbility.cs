using UnityEngine;

namespace ClementTodd.Characters
{
    public class MoveAbility : Ability
    {
        public float speed = 2f;
        public float runSpeed = 4f;

        public float turnSpeed = 360f;

        public Transform groundAnchor;

        private Vector3 moveVelocity = Vector3.zero;

        private GravityReceiver gravityReceiver;

        private const float minMove = 0.001f;

        private const float maxGroundDistance = 0.2f;


        private void Awake()
        {
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        private void FixedUpdate()
        {
            if (behaviourData.move.sqrMagnitude >= minMove || moveVelocity.sqrMagnitude >= minMove)
            {
                Vector3 moveDirection = new Vector3(behaviourData.move.x, 0f, behaviourData.move.y);
                float speed = behaviourData.run ? runSpeed : this.speed;
                float maxAcceleration = speed / Time.fixedDeltaTime;
                float friction = (gravityReceiver != null) ? gravityReceiver.groundFriction : 1f;
                moveVelocity = Vector3.MoveTowards(moveVelocity, moveDirection * speed, maxAcceleration * friction * Time.fixedDeltaTime);

                if (!Mathf.Approximately(moveDirection.sqrMagnitude, 0f))
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, transform.up);
                    character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
                }

                Vector3 movement = moveVelocity;

                if (gravityReceiver && gravityReceiver.isGrounded)
                {
                    // Align movement to ground for better control
                    movement = gravityReceiver.AlignVectorToGround(movement);

                    // Push slightly into the ground to avoid awkward hops when the slope angle changes
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

                character.body.MovePosition(character.transform.position + movement * Time.fixedDeltaTime);
            }
        }
    }
}