﻿using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd.Environment.Gravity
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityReceiver : MonoBehaviour
    {
        private Rigidbody body;

        public float gravityScale = 1f;

        public bool stayUpright = true;

        private List<Collider> groundColliders = new List<Collider>();

        public bool isGrounded { get; private set; }

        public Vector3 groundNormal { get; private set; }

        private Vector3 totalGroundNormal = Vector3.zero;

        public float groundFriction { get; private set; }

        private float totalGroundFriction = 0f;

        public Vector3 groundContactPoint { get; private set; }

        private Vector3 totalGroundContactPoint = Vector3.zero;

        public Vector3 gravity { get; private set; }
        public Vector3 gravityDirection { get; private set; }
        public float gravityMagnitude { get; private set; }

        public float groundSlope { get; private set; }

        public Vector3 horizontalSlopeDirection { get; private set; }

        [Range(0f, 1f)]
        public float minSlopeGrip = 0.7f;

        private const float minSlopeToSlide = 0.1f;
        public float gripOnGround { get; private set; }

        public bool isSliding { get; private set; }

        const float uprightTurnSpeed = 360f;

        const float minGroundDotProduct = 0.35f;

        const float minAirtimeAfterLaunch = 0.1f;
        private float lastLaunchTime = -1f;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            bool applyGravity = body.useGravity && !Mathf.Approximately(gravityScale, 0f);

            if (applyGravity)
            {
                gravity = GravityManager.instance.GetGravityAtPosition(body.centerOfMass);
                gravityDirection = gravity.normalized;
                gravityMagnitude = gravity.magnitude;
            }
            else
            {
                gravity = Vector3.zero;
                gravityDirection = Vector3.zero;
                gravityMagnitude = 0f;
            }

            // Check if gravity receiver is grounded
            UpdateIsGrounded();
            if (isGrounded)
            {
                groundNormal = (totalGroundNormal / groundColliders.Count).normalized;
                groundFriction = totalGroundFriction / groundColliders.Count;
                groundContactPoint = totalGroundContactPoint / groundColliders.Count;
                groundSlope = 1f - Vector3.Dot(groundNormal, -gravityDirection);
            }
            else
            {
                groundNormal = -gravityDirection;
                groundSlope = 1f;
            }

            groundColliders.Clear();
            totalGroundNormal = Vector3.zero;
            totalGroundFriction = 0f;
            totalGroundContactPoint = Vector3.zero;

            if (applyGravity)
            {
                Vector3 gravityToApply = gravity;

                if (isGrounded)
                {
                    // If the slope is too steep or slippery, slide down in a controlled manner.
                    gripOnGround = groundFriction * (1f - groundSlope);
                    isSliding = gripOnGround < minSlopeGrip && groundSlope >= minSlopeToSlide;
                    horizontalSlopeDirection = AlignVectorToHorizontalPlane(groundNormal).normalized;

                    if (isSliding)
                    {
                        Vector3 slideDirection = AlignVectorToGround(horizontalSlopeDirection).normalized;
                        gravityToApply = slideDirection * gravityMagnitude * (1f - gripOnGround);
                    }
                    else
                    {
                        // Cheat by applying gravity directly into the ground's
                        // slope (instead of downwards) to prevent unintended sliding.
                        gravityToApply = -groundNormal * gravityMagnitude;
                    }
                }
                else
                {
                    gripOnGround = 0f;
                    isSliding = false;
                    horizontalSlopeDirection = Vector3.zero;
                }

                // Apply gravity to receiver
                body.AddForce(gravityToApply * gravityScale);

                // Rotate to stay upright
                if (stayUpright && !Mathf.Approximately(gravity.sqrMagnitude, 0f))
                {
                    Quaternion targetRotation = Quaternion.LookRotation(transform.forward, -gravityDirection);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        uprightTurnSpeed * Time.fixedDeltaTime);
                }
            }
        }

        private void UpdateIsGrounded()
        {
            isGrounded = groundColliders.Count > 0 && gravityMagnitude > 0f && Time.time >= lastLaunchTime + minAirtimeAfterLaunch;
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision);
        }

        private void HandleCollision(Collision collision)
        {
            // See if any contact point with this collider is below the gravity receiver
            if (!groundColliders.Contains(collision.collider))
            {
                for (int i = 0; i < collision.contactCount; i++)
                {
                    ContactPoint contact = collision.GetContact(i);
                    float dot = Vector3.Dot(contact.normal, -gravityDirection);

                    if (dot >= minGroundDotProduct)
                    {
                        groundColliders.Add(collision.collider);
                        totalGroundNormal += contact.normal;
                        totalGroundFriction += collision.collider.material.staticFriction;
                        totalGroundContactPoint += contact.point;

                        UpdateIsGrounded();

                        return;
                    }
                }
            }
        }

        public void LaunchUpwards(float force)
        {
            body.AddForce(-gravityDirection * force);
            isGrounded = false;
            lastLaunchTime = Time.time;
        }

        public Vector3 AlignVectorToGround(Vector3 vector)
        {
            if (isGrounded)
            {
                float groundDot = Vector3.Dot(vector, groundNormal);
                vector -= groundNormal * groundDot;
            }

            return vector;
        }

        public Vector3 AlignVectorToHorizontalPlane(Vector3 vector)
        {
            float planeDot = Vector3.Dot(vector, gravityDirection);
            vector -= gravityDirection * planeDot;
            return vector;
        }
    }
}