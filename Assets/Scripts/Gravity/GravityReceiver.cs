using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    private Rigidbody body;

    public float gravityScale = 1f;

    public bool stayUpright = true;

    const float uprightTurnSpeed = 180f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 gravity = GravityManager.instance.GetGravityAtPosition(body.centerOfMass);

        body.AddForce(gravity * gravityScale);

        if (stayUpright && !Mathf.Approximately(gravity.sqrMagnitude, 0f))
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, -gravity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, uprightTurnSpeed * Time.fixedDeltaTime);
        }
    }
}
