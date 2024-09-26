using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed;
    public Transform cameraTransform;
    public Vector3 rotationPoint;
    public float movementSpeed;
    public float shiftMultipier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) //Mouse left click 
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            cameraTransform.Rotate(Vector3.up, mouseX, Space.World);
            cameraTransform.Rotate(Vector3.right, -mouseY, Space.Self);

        }

        float currentSpeed = movementSpeed;

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultipier;
        }

        Vector3 forwardMovement = cameraTransform.forward * currentSpeed * Time.deltaTime;
        Vector3 backwardMovement = -cameraTransform.forward * currentSpeed * Time.deltaTime;
        Vector3 rightMovement = cameraTransform.right * currentSpeed * Time.deltaTime;
        Vector3 leftMovement = -cameraTransform.right * currentSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            cameraTransform.position += forwardMovement;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraTransform.position += leftMovement;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraTransform.position += backwardMovement;
        }
        if (Input.GetKey(KeyCode.D))
        {
            cameraTransform.position += rightMovement;
        }

    }
}
