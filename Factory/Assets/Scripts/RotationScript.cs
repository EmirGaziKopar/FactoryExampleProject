using System.Collections;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float rotationSpeed = 360f; // Matkabýn dönüþ hýzý
    public float shakeIntensity = 0.15f;  // Titreme yoðunluðu
    public float shakeFrequency = 0.1f; // Titreme sýklýðý (saniye)
    public float activeDuration = 3f;  // Matkabýn dönüp titrediði süre (saniye)
    public float idleDuration = 2f;    // Matkabýn durduðu süre (saniye)

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
            // Dönme hareketi
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
            transform.position = originalPosition; // Titreme bittiðinde pozisyonu sýfýrla
            yield return new WaitForSeconds(idleDuration);
        }
    }
}
