using UnityEngine;

namespace ClementTodd.Characters
{
    public class MoveAbility : Ability
    {
        public float speed = 2f;
        public float runSpeed = 4f;

        public float turnSpeed = 360f;

        private GravityReceiver gravityReceiver;

        private float groundStickiness = 4f;

        private const float minMove = 0.001f;


        private void Awake()
        {
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        private void FixedUpdate()
        {
            if (behaviourData.move.sqrMagnitude >= minMove)
            {
                Vector3 moveDirection = new Vector3(behaviourData.move.x, 0f, behaviourData.move.y);
                float speed = behaviourData.run ? runSpeed : this.speed;
                Vector3 movement = moveDirection * speed;

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, transform.up);
                character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

                if (gravityReceiver && gravityReceiver.isGrounded)
                {
                    // Align movement to ground for better control
                    movement = gravityReceiver.AlignVectorToGround(movement);

                    // Push slightly into the ground to avoid awkward hops when the slope angle changes
                    movement -= gravityReceiver.groundNormal * groundStickiness;
                }
                character.body.MovePosition(character.transform.position + movement * Time.fixedDeltaTime);
            }
        }
    }
}