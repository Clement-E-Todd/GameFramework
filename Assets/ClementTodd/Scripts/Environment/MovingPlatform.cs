using UnityEngine;

namespace ClementTodd
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        public Vector3 endPosition;
        public float moveDuration;
        public float pauseDuration;
        public AnimationCurve moveCurve;

        private Rigidbody body;
        private Vector3 startPosition;
        private float moveProgress = 0f;
        private bool reverseMove = false;
        private float lastPauseStartTime = 0f;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            startPosition = transform.position;
            lastPauseStartTime = Time.time;
        }

        private void FixedUpdate()
        {
            bool paused = (lastPauseStartTime > Time.time - pauseDuration);

            if (!paused)
            {
                moveProgress += (reverseMove ? -Time.fixedDeltaTime : Time.fixedDeltaTime) / moveDuration;
                if (moveProgress < 0f || moveProgress > 1f)
                {
                    moveProgress = Mathf.Clamp01(moveProgress);
                    reverseMove = !reverseMove;
                    lastPauseStartTime = Time.time;
                }

                float curveProgress = moveCurve.Evaluate(moveProgress);

                body.MovePosition(Vector3.Lerp(startPosition, endPosition, curveProgress));
            }
        }
    }
}