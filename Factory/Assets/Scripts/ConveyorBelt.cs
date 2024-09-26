using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float conveyorSpeed = 5f; // Bandýn hýzý
    private Vector3 moveDirection;

    void Start()
    {
        // Ýlk baþta bandýn hareket yönünü belirleyin (örneðin, z ekseni boyunca)
        moveDirection = transform.right;
    }

    void Update()
    {
        // Her karede bandýn hareket yönünü güncelleyin
        moveDirection = transform.right;
    }

    void OnTriggerStay(Collider other)
    {
        // Kinematik rigidbody olan objeleri hareket ettirin
        if (other.attachedRigidbody != null && other.attachedRigidbody.isKinematic)
        {
            other.transform.position += moveDirection * conveyorSpeed * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection * 5);
    }
}
