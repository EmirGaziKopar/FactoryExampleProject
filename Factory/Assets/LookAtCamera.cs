using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtCamera : MonoBehaviour
{

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(mainCamera != null)
        {
            Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.forward;
            Vector3 targetUp = mainCamera.transform.rotation * Vector3.up;
            transform.LookAt(targetPosition, targetUp);
        }

        
    }
}
