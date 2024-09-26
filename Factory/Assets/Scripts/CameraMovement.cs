using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float movementSpeed = 5f;
    public float shiftMultiplier = 2f; // Shift'e basýldýðýnda hýz artýþý için çarpan
    public Transform cameraTransform;

    private bool rotateFromTouch = false;
    private Vector3 rotationPoint;

    void Update()
    {
        // Fare ile kamerayý döndürme
        if (Input.GetMouseButton(1)) // Sað týk ile döndürme
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            cameraTransform.Rotate(Vector3.up, mouseX, Space.World);
            cameraTransform.Rotate(Vector3.right, -mouseY, Space.Self);
        }

        // Dokunmatik hareket ile kamera döndürme
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

        if (Input.GetMouseButtonDown(0)) // Sol týklama ile rotasyon baþlasýn
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                rotationPoint = hit.point; // Dokunduðunuz noktayý al
            }
        }

        if (rotationPoint != Vector3.zero)
        {
            // Rotasyonu o noktadan yap (örneðin Y ekseninde)
            cameraTransform.RotateAround(rotationPoint, Vector3.up, 20 * Time.deltaTime);
        }

        // Hareket etme mekanikleri
        float currentSpeed = movementSpeed;

        // Shift'e basýldýðýnda hýz artýþý
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultiplier; // Hýzý çarpan ile artýr
        }
        //cameraTransform.rotation * new Vector3(0,0,1) cameraTransform.Forward
        Vector3 forwardMovement = cameraTransform.rotation * new Vector3(0,0,1) * currentSpeed * Time.deltaTime;
        Vector3 backwardMovement = cameraTransform.rotation * new Vector3(0, 0, -1) * currentSpeed * Time.deltaTime;
        Vector3 rightMovement = cameraTransform.right * currentSpeed * Time.deltaTime;
        Vector3 leftMovement = -cameraTransform.right * currentSpeed * Time.deltaTime;

        // Ýleri gitme (W tuþu)
        if (Input.GetKey(KeyCode.W))
        {
            cameraTransform.position += forwardMovement;
        }

        // Geri gitme (S tuþu)
        if (Input.GetKey(KeyCode.S))
        {
            cameraTransform.position += backwardMovement;
        }

        // Saða gitme (D tuþu)
        if (Input.GetKey(KeyCode.D))
        {
            cameraTransform.position += rightMovement;
        }

        // Sola gitme (A tuþu)
        if (Input.GetKey(KeyCode.A))
        {
            cameraTransform.position += leftMovement;
        }
    }
}
