using ClementTodd.Environment.Gravity;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    [RequireComponent(typeof(GravityReceiver))]
    public class MoveAbility : Ability
    {
        public float speed = 6f;
        public float runSpeed = 10f;

        public float turnSpeed = 360f;
        public float runTurnSpeed = 180f;

        public float airFriction = 0.05f;

        public float maxUphillResistance = 20f;

        public Transform groundAnchor;

        private Vector2 intendedMove = Vector2.zero;

        private bool isRunning = false;

        private Vector3 moveVelocity = Vector3.zero;

        private GravityReceiver gravityReceiver;

        private const float minMove = 0.001f;

        private const float maxGroundDistance = 0.2f;

        private const float ledgeSlipSpeed = 2f;


        private void Awake()
        {
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        public void SetIntendedMove(Vector2 intendedMove)
        {
            this.intendedMove = intendedMove;
        }

        public void SetIsRunning(bool run)
        {
            isRunning = run;
        }

        private void FixedUpdate()
        {
            // Check whether the character is hanging off of a ledge so they can slip off
            bool isSlippingOffLedge = false;
            RaycastHit groundAnchorHit = default;
            if (groundAnchor)
            {
                isSlippingOffLedge = !Physics.Raycast(
                    groundAnchor.position,
                    gravityReceiver.gravityDirection,
                    out groundAnchorHit,
                    maxGroundDistance);
            }

            if (intendedMove.sqrMagnitude >= minMove || moveVelocity.sqrMagnitude >= minMove || isSlippingOffLedge)
            {
                // Calculate the variables needed to move based on the character's human-or-computer-controller behaviour
                Vector3 moveDirection = new Vector3(intendedMove.x, 0f, intendedMove.y);
                float speed = isRunning ? runSpeed : this.speed;
                float turnSpeed = isRunning ? runTurnSpeed : this.turnSpeed;
                float friction = gravityReceiver.isGrounded ? gravityReceiver.gripOnGround : Mathf.Min(gravityReceiver.groundFriction, airFriction);

                // If running, character's movement turns only as fast as the character rotates.
                if (isRunning)
                {
                    moveDirection = Vector3.RotateTowards(transform.forward, moveDirection, turnSpeed * Time.fixedDeltaTime * Mathf.Deg2Rad, float.MaxValue);
                }

                // Accelerate the character's move velocity towards their inteded velocity.
                moveVelocity = Vector3.MoveTowards(moveVelocity, moveDirection * speed, speed * friction);

                // If sliding down a slope, push move velocity downwards to prevent the character from unintentionally lifting off of the ground
                if (gravityReceiver.isSliding)
                {
                    float slideDot = Vector3.Dot(moveVelocity, gravityReceiver.horizontalSlopeDirection);
                    if (slideDot < 0f)
                    {
                        Vector3 targetSlideVelocity = moveVelocity - (gravityReceiver.horizontalSlopeDirection * slideDot);
                        moveVelocity = Vector3.MoveTowards(moveVelocity, targetSlideVelocity, maxUphillResistance * (1f - gravityReceiver.gripOnGround));
                    }
                }

                // If the character's move velocity opposes their Rigidbody's velocity, reduce both the move and Rigidbody velocity.
                // Only works on the ground because their is no traction in the air.
                if (gravityReceiver.isGrounded && !Mathf.Approximately(moveVelocity.sqrMagnitude, 0f))
                {
                    Vector3 velocityDirection = character.body.velocity.normalized;
                    float opposingVelocity = -Vector3.Dot(moveVelocity, velocityDirection);

                    if (opposingVelocity > 0f)
                    {
                        Vector3 neutralizingForce = -velocityDirection * Mathf.Min(opposingVelocity, character.body.velocity.magnitude) * (1f - gravityReceiver.groundSlope);

                        character.body.velocity += neutralizingForce;
                        moveVelocity -= neutralizingForce;
                    }
                }

                // Turn to face the direction that the character *intends* to move in.
                if (!Mathf.Approximately(moveDirection.sqrMagnitude, 0f))
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, transform.up);
                    character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
                }

                // Calculate the actual amount by which the character should move
                Vector3 movement = moveVelocity;

                if (gravityReceiver && gravityReceiver.isGrounded)
                {
                    // Align movement to ground for better control
                    movement = gravityReceiver.AlignVectorToGround(movement);

                    // If hanging off of a ledge, slip off.
                    if (isSlippingOffLedge)
                    {
                        Vector3 ledgeSlipDirection = gravityReceiver.AlignVectorToHorizontalPlane(groundAnchor.position - gravityReceiver.groundContactPoint).normalized;
                        movement += ledgeSlipDirection * ledgeSlipSpeed;
                    }

                    // Push lightly into the ground to avoid awkward hops when the slope angle changes
                    else
                    {
                        movement -= gravityReceiver.groundNormal * groundAnchorHit.distance / Time.fixedDeltaTime;
                    }
                }

                // Move the character by the calculated amount
                character.body.MovePosition(character.transform.position + movement * Time.fixedDeltaTime);
            }
        }
    }
}