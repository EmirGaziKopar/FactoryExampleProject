using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Vector3 localPushDirection = Vector3.right; // Varsay�lan olarak sa�a ittirir
    public float pushSpeed = 5f; // �tme h�z�

    void OnTriggerStay(Collider other)
    {
        // Kinematik rigidbody olan objeleri hareket ettirin
        if (other.attachedRigidbody != null && other.attachedRigidbody.isKinematic)
        {
            // Yerel y�n� d�nya y�n�ne d�n��t�r
            Vector3 worldPushDirection = transform.TransformDirection(localPushDirection);
            other.transform.position += worldPushDirection * pushSpeed * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 worldPushDirection = transform.TransformDirection(localPushDirection);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, worldPushDirection * 5);
    }
}
