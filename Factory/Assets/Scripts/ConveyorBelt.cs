using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float conveyorSpeed = 5f; // Band�n h�z�
    private Vector3 moveDirection;

    void Start()
    {
        // �lk ba�ta band�n hareket y�n�n� belirleyin (�rne�in, z ekseni boyunca)
        moveDirection = transform.right;
    }

    void Update()
    {
        // Her karede band�n hareket y�n�n� g�ncelleyin
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
