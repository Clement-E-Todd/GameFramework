using UnityEngine;

namespace ClementTodd.Characters
{
    public class MoveAbility : Ability
    {
        public float speed = 2f;
        public float runSpeed = 4f;

        public float turnSpeed = 360f;

        private const float minMove = 0.001f;

        private GravityReceiver gravityReceiver;

        private void Awake()
        {
            gravityReceiver = GetComponent<GravityReceiver>();
        }

        private void FixedUpdate()
        {
            if (behaviourData.move.sqrMagnitude >= minMove)
            {
                float speed = behaviourData.run ? runSpeed : this.speed;
                Vector3 moveDirection = new Vector3(behaviourData.move.x, 0f, behaviourData.move.y);

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, transform.up);
                character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

                if (gravityReceiver)
                {
                    moveDirection = gravityReceiver.AlignVectorToGround(moveDirection);
                }
                character.body.MovePosition(character.transform.position + moveDirection * speed * Time.fixedDeltaTime);
            }
        }
    }
}