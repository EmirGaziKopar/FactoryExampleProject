using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnPhysics : MonoBehaviour
{
    public Vector3 forceDirection = new Vector3(0, 0, 1); // Kuvvet y�n� (varsay�lan olarak ileri do�ru)
    public float forceMagnitude = 10f; // Kuvvet b�y�kl���

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            Vector3 oppositeForceDirection = -forceDirection.normalized; // Kuvvet y�n�n�n tersi
            rb.AddForce(oppositeForceDirection * forceMagnitude, ForceMode.Impulse); // Kuvveti uygula
        }
    }
}
