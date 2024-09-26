using System.Collections;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationSpeed = 360f; // Matkab�n d�n�� h�z�
    public float shakeIntensity = 0.15f;  // Titreme yo�unlu�u
    public float shakeFrequency = 0.1f; // Titreme s�kl��� (saniye)
    public float activeDuration = 3f;  // Matkab�n d�n�p titredi�i s�re (saniye)
    public float idleDuration = 2f;    // Matkab�n durdu�u s�re (saniye)

    private bool isActive = false;
    private float timer = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(RotateAndShake());
    }

    void Update()
    {
        if (isActive)
        {
            // D�nme hareketi
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // Titreme hareketi
            float shakeOffsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float shakeOffsetY = Random.Range(-shakeIntensity, shakeIntensity);
            transform.position = originalPosition + new Vector3(shakeOffsetX, shakeOffsetY, 0f);
        }
    }

    IEnumerator RotateAndShake()
    {
        while (true)
        {
            // Aktif durum
            isActive = true;
            yield return new WaitForSeconds(activeDuration);

            // Pasif durum
            isActive = false;
            transform.position = originalPosition; // Titreme bitti�inde pozisyonu s�f�rla
            yield return new WaitForSeconds(idleDuration);
        }
    }
}
