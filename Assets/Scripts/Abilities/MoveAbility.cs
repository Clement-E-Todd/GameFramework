using UnityEngine;

namespace ClementTodd.Characters
{
    public class MoveAbility : Ability
    {
        public float speed = 2f;

        private Vector2 moveVector;

        private void Update()
        {
            moveVector = behaviourData.moveDirection * speed;
        }

        private void FixedUpdate()
        {
            character.controller.Move(new Vector3(moveVector.x, 0f, moveVector.y) * Time.fixedDeltaTime);
        }
    }
}