using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Vector3 localPushDirection = Vector3.right; // Varsayýlan olarak saða ittirir
    public float pushSpeed = 5f; // Ýtme hýzý

    void OnTriggerStay(Collider other)
    {
        // Kinematik rigidbody olan objeleri hareket ettirin
        if (other.attachedRigidbody != null && other.attachedRigidbody.isKinematic)
        {
            // Yerel yönü dünya yönüne dönüþtür
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
