using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Kameray� al�yoruz
        mainCamera = Camera.main;
    }

    void Update()
    {
        // UI elementinin kameraya bakmas�n� sa�l�yoruz
        if (mainCamera != null)
        {
            // Kameran�n ters y�n�ne bakmas� i�in y�n� ayarl�yoruz
            Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.forward;
            Vector3 targetUp = mainCamera.transform.rotation * Vector3.up;

            // UI elementini kameraya bakacak �ekilde d�nd�r
            transform.LookAt(targetPosition, targetUp);
        }
    }
}
