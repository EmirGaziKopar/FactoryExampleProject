using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProductMovementManager : MonoBehaviour
{
    public GameObject productPrefab;  // �r�n prefab'�
    public Transform spawnPoint;      // �r�nlerin spawn olaca�� nokta
    public Transform[] waypoints;     // �r�nlerin hareket edece�i waypoint'ler
    public float spawnInterval = 5.0f; // �r�nlerin spawn aral��� (saniye)
    public float moveSpeed = 2.0f;    // �r�n hareket h�z�
    public float rotationSpeed = 2.0f; // �r�nlerin hedefe bakma h�z�n� ayarlar (Lerp h�z�n� kontrol eder)

    // Animasyon ve duraklama parametreleri
    public Animation robotArmAnimation;  // �lk bekleme noktas�nda �al��acak animasyon
    public Animation secondAnimation;    // �kinci bekleme noktas�nda �al��acak animasyon
    public float waitTimeFirst = 3.0f;   // �lk bekleme noktas� i�in bekleme s�resi
    public float waitTimeSecond = 2.0f;  // �kinci bekleme noktas� i�in bekleme s�resi

    // Kaynak ve �r�n y�netimi
    public int totalResources = 100;  // Ba�lang��taki toplam kaynak miktar�
    public int resourceAmountNeeded = 5; // Her �r�n i�in gereken kaynak
    public Text productCounterText;   // �retilen �r�n sayac�n� g�sterecek Text (null olabilir)

    // ��sel durum
    private int totalProductsProduced = 0;  // �retilen toplam �r�n say�s�
    private bool isSpawning = true;         // �r�nlerin spawn olup olmad���n� kontrol eder
    private bool isProductMoving = false;   // �r�n hareket halindeyken yeni �r�n spawn edilmesini engeller

    private GameObject currentProduct;      // �u anda hareket eden �r�n
    private int currentWaypointIndex = 0;   // �r�nlerin hareket edece�i waypoint'leri takip eder
    private bool isWaiting = false;         // �r�n duraklad���nda bu de�eri true yapar

    void Start()
    {
        UpdateProductCounter();
        StartCoroutine(SpawnProducts());
    }




    IEnumerator SpawnProducts()
    {
        // Kaynak yetti�i s�rece �r�n �ret
        while (isSpawning && totalResources >= resourceAmountNeeded)
        {
            // E�er �r�n hareket halindeyse yeni �r�n �retme
            if (!isProductMoving && totalResources >= resourceAmountNeeded)
            {
                // Kaynaklar� azalt
                totalResources -= resourceAmountNeeded;

                // Yeni �r�n SpawnPoint'te �ret
                currentProduct = Instantiate(productPrefab, spawnPoint.position, Quaternion.identity);

                // �r�n say�s�n� art�r ve UI'yi g�ncelle
                totalProductsProduced++;
                UpdateProductCounter();

                // �r�n hareket etmeye ba�l�yor, bu y�zden yeni �r�n �retilmesini engelle
                isProductMoving = true;

                // �r�n� hareket ettir
                currentWaypointIndex = 0;  // �r�n ilk waypoint'ten ba�layacak
                StartCoroutine(MoveProduct());
            }

            // Bir sonraki �r�n spawnlanmadan �nce bekle (ama bekleme durumunu kontrol edece�iz)
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveProduct()
    {
        // �r�n waypoint'ler boyunca hareket ederken
        while (currentWaypointIndex < waypoints.Length)
        {
            // E�er �r�n hareket ediyorsa ve bekleme durumunda de�ilse
            if (!isWaiting)
            {
                Transform targetWaypoint = waypoints[currentWaypointIndex];

                // �r�n hedef waypoint'e do�ru hareket et
                currentProduct.transform.position = Vector3.MoveTowards(currentProduct.transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

                // �r�n�n hedef waypoint'e bakmas�n� sa�la (yumu�ak Lerp ile)
                Vector3 lookDirection = targetWaypoint.position - currentProduct.transform.position;
                lookDirection.y = 0; // Y ekseninde d�nmesini engelle (sadece yatayda d�n��)
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                currentProduct.transform.rotation = Quaternion.Lerp(currentProduct.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Waypoint'e ula�t���nda bir sonraki waypoint'e ge�
                if (Vector3.Distance(currentProduct.transform.position, targetWaypoint.position) < 0.1f)
                {
                    currentWaypointIndex++; // �ndex'i bir sonraki waypoint'e ge�mek i�in art�r

                    // E�er 1. durma noktas�na geldiysek
                    if (currentWaypointIndex == 5)
                    {
                        yield return StartCoroutine(WaitAndPlayFirstAnimation());
                    }

                    // E�er 2. durma noktas�na geldiysek
                    if (currentWaypointIndex == 4)
                    {
                        yield return StartCoroutine(WaitAndPlaySecondAnimation());
                    }
                }
            }

            // Bir sonraki kareyi bekle
            yield return null;
        }

        // �r�n EndPoint'e ula�t���nda hareket biter, yeni �r�n spawn edilebilir
        isProductMoving = false;
    }

    IEnumerator WaitAndPlayFirstAnimation()
    {
        isWaiting = true;

        // �lk animasyonu oynat
        robotArmAnimation.Play("RobotArm");

        // �lk bekleme s�resince animasyon oynar
        yield return new WaitForSeconds(waitTimeFirst);

        // Hareket devam eder
        isWaiting = false;
    }

    IEnumerator WaitAndPlaySecondAnimation()
    {
        isWaiting = true;

        // �kinci animasyonu oynat
        secondAnimation.Play("Line06");

        // �kinci bekleme s�resince animasyon oynar
        yield return new WaitForSeconds(waitTimeSecond);

        // Hareket devam eder
        isWaiting = false;
    }

    void UpdateProductCounter()
    {
        // Text'in null olup olmad���n� kontrol et
        if (productCounterText != null)
        {
            productCounterText.text = "�retilen �r�n: " + totalProductsProduced.ToString();
        }
    }
}
