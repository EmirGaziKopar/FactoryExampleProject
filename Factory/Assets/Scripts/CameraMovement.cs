using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float movementSpeed = 5f;
    public float shiftMultiplier = 2f; // Shift'e bas�ld���nda h�z art��� i�in �arpan
    public Transform cameraTransform;

    private bool rotateFromTouch = false;
    private Vector3 rotationPoint;

    void Update()
    {
        // Fare ile kameray� d�nd�rme
        if (Input.GetMouseButton(1)) // Sa� t�k ile d�nd�rme
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            cameraTransform.Rotate(Vector3.up, mouseX, Space.World);
            cameraTransform.Rotate(Vector3.right, -mouseY, Space.Self);
        }

        // Dokunmatik hareket ile kamera d�nd�rme
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float touchX = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                float touchY = touch.deltaPosition.y * rotationSpeed * Time.deltaTime;

                cameraTransform.Rotate(Vector3.up, touchX, Space.World);
                cameraTransform.Rotate(Vector3.right, -touchY, Space.Self);
            }
        }

        if (Input.GetMouseButtonDown(0)) // Sol t�klama ile rotasyon ba�las�n
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                rotationPoint = hit.point; // Dokundu�unuz noktay� al
            }
        }

        if (rotationPoint != Vector3.zero)
        {
            // Rotasyonu o noktadan yap (�rne�in Y ekseninde)
            cameraTransform.RotateAround(rotationPoint, Vector3.up, 20 * Time.deltaTime);
        }

        // Hareket etme mekanikleri
        float currentSpeed = movementSpeed;

        // Shift'e bas�ld���nda h�z art���
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultiplier; // H�z� �arpan ile art�r
        }
        //cameraTransform.rotation * new Vector3(0,0,1) cameraTransform.Forward
        Vector3 forwardMovement = cameraTransform.rotation * new Vector3(0,0,1) * currentSpeed * Time.deltaTime;
        Vector3 backwardMovement = cameraTransform.rotation * new Vector3(0, 0, -1) * currentSpeed * Time.deltaTime;
        Vector3 rightMovement = cameraTransform.right * currentSpeed * Time.deltaTime;
        Vector3 leftMovement = -cameraTransform.right * currentSpeed * Time.deltaTime;

        // �leri gitme (W tu�u)
        if (Input.GetKey(KeyCode.W))
        {
            cameraTransform.position += forwardMovement;
        }

        // Geri gitme (S tu�u)
        if (Input.GetKey(KeyCode.S))
        {
            cameraTransform.position += backwardMovement;
        }

        // Sa�a gitme (D tu�u)
        if (Input.GetKey(KeyCode.D))
        {
            cameraTransform.position += rightMovement;
        }

        // Sola gitme (A tu�u)
        if (Input.GetKey(KeyCode.A))
        {
            cameraTransform.position += leftMovement;
        }
    }
}
