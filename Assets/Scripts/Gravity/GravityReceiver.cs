using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    private Rigidbody body;

    public float gravityScale = 1f;

    public bool stayUpright = true;

    private List<Collider> groundColliders = new List<Collider>();

    public bool isGrounded { get; private set; }

    private Vector3 groundNormal = Vector3.up;

    private Vector3 totalGroundNormal = Vector3.zero;

    public Vector3 gravityDirection { get; private set; }

    const float uprightTurnSpeed = 360f;

    const float minGroundDotProduct = 0.7f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Apply gravity to receiver
        if (body.useGravity && !Mathf.Approximately(gravityScale, 0f))
        {
            Vector3 gravity = GravityManager.instance.GetGravityAtPosition(body.centerOfMass);
            gravityDirection = gravity.normalized;

            body.AddForce(gravity * gravityScale);

            // Rotate to stay upright
            if (stayUpright && !Mathf.Approximately(gravity.sqrMagnitude, 0f))
            {
                Quaternion targetRotation = Quaternion.LookRotation(transform.forward, -gravityDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, uprightTurnSpeed * Time.fixedDeltaTime);
            }
        }

        // Check if gravity receiver is grounded
        isGrounded = groundColliders.Count > 0;
        groundNormal = isGrounded ? (totalGroundNormal / groundColliders.Count).normalized : -gravityDirection;

        groundColliders.Clear();
        totalGroundNormal = Vector3.zero;
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
                    isGrounded = true;

                    groundColliders.Add(collision.collider);
                    totalGroundNormal += contact.normal;

                    return;
                }
            }
        }
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
}
