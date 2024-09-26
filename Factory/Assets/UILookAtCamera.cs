using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Kamerayý alýyoruz
        mainCamera = Camera.main;
    }

    void Update()
    {
        // UI elementinin kameraya bakmasýný saðlýyoruz
        if (mainCamera != null)
        {
            // Kameranýn ters yönüne bakmasý için yönü ayarlýyoruz
            Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.forward;
            Vector3 targetUp = mainCamera.transform.rotation * Vector3.up;

            // UI elementini kameraya bakacak þekilde döndür
            transform.LookAt(targetPosition, targetUp);
        }
    }
}
