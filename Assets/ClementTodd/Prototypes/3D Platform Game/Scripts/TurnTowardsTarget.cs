using UnityEngine;

public class TurnTowardsTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(
            (target.transform.position - transform.position).normalized,
            Vector3.up);
    }
}
