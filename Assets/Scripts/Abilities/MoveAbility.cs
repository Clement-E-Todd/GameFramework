using UnityEngine;

namespace ClementTodd.Characters
{
    public class MoveAbility : Ability
    {
        public float speed = 2f;

        public float turnSpeed = 360f;

        private const float minMove = 0.001f;
        private void FixedUpdate()
        {
            if (behaviourData.move.sqrMagnitude >= minMove)
            {
                Vector3 moveDirection = new Vector3(behaviourData.move.x, 0f, behaviourData.move.y);
                character.body.MovePosition(character.transform.position + moveDirection * speed * Time.fixedDeltaTime);

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
                character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            }
        }
    }
}