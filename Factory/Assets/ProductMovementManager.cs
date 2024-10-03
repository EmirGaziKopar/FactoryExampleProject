using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProductMovementManager : MonoBehaviour
{
    public GameObject productPrefab;  // Ürün prefab'ý
    public Transform spawnPoint;      // Ürünlerin spawn olacaðý nokta
    public Transform[] waypoints;     // Ürünlerin hareket edeceði waypoint'ler
    public float spawnInterval = 5.0f; // Ürünlerin spawn aralýðý (saniye)
    public float moveSpeed = 2.0f;    // Ürün hareket hýzý
    public float rotationSpeed = 2.0f; // Ürünlerin hedefe bakma hýzýný ayarlar (Lerp hýzýný kontrol eder)

    // Animasyon ve duraklama parametreleri
    public Animation robotArmAnimation;  // Ýlk bekleme noktasýnda çalýþacak animasyon
    public Animation secondAnimation;    // Ýkinci bekleme noktasýnda çalýþacak animasyon
    public float waitTimeFirst = 3.0f;   // Ýlk bekleme noktasý için bekleme süresi
    public float waitTimeSecond = 2.0f;  // Ýkinci bekleme noktasý için bekleme süresi

    // Kaynak ve ürün yönetimi
    public int totalResources = 100;  // Baþlangýçtaki toplam kaynak miktarý
    public int resourceAmountNeeded = 5; // Her ürün için gereken kaynak
    public Text productCounterText;   // Üretilen ürün sayacýný gösterecek Text (null olabilir)

    // Ýçsel durum
    private int totalProductsProduced = 0;  // Üretilen toplam ürün sayýsý
    private bool isSpawning = true;         // Ürünlerin spawn olup olmadýðýný kontrol eder
    private bool isProductMoving = false;   // Ürün hareket halindeyken yeni ürün spawn edilmesini engeller

    private GameObject currentProduct;      // Þu anda hareket eden ürün
    private int currentWaypointIndex = 0;   // Ürünlerin hareket edeceði waypoint'leri takip eder
    private bool isWaiting = false;         // Ürün durakladýðýnda bu deðeri true yapar

    void Start()
    {
        UpdateProductCounter();
        StartCoroutine(SpawnProducts());
    }




    IEnumerator SpawnProducts()
    {
        // Kaynak yettiði sürece ürün üret
        while (isSpawning && totalResources >= resourceAmountNeeded)
        {
            // Eðer ürün hareket halindeyse yeni ürün üretme
            if (!isProductMoving && totalResources >= resourceAmountNeeded)
            {
                // Kaynaklarý azalt
                totalResources -= resourceAmountNeeded;

                // Yeni ürün SpawnPoint'te üret
                currentProduct = Instantiate(productPrefab, spawnPoint.position, Quaternion.identity);

                // Ürün sayýsýný artýr ve UI'yi güncelle
                totalProductsProduced++;
                UpdateProductCounter();

                // Ürün hareket etmeye baþlýyor, bu yüzden yeni ürün üretilmesini engelle
                isProductMoving = true;

                // Ürünü hareket ettir
                currentWaypointIndex = 0;  // Ürün ilk waypoint'ten baþlayacak
                StartCoroutine(MoveProduct());
            }

            // Bir sonraki ürün spawnlanmadan önce bekle (ama bekleme durumunu kontrol edeceðiz)
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveProduct()
    {
        // Ürün waypoint'ler boyunca hareket ederken
        while (currentWaypointIndex < waypoints.Length)
        {
            // Eðer ürün hareket ediyorsa ve bekleme durumunda deðilse
            if (!isWaiting)
            {
                Transform targetWaypoint = waypoints[currentWaypointIndex];

                // Ürün hedef waypoint'e doðru hareket et
                currentProduct.transform.position = Vector3.MoveTowards(currentProduct.transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

                // Ürünün hedef waypoint'e bakmasýný saðla (yumuþak Lerp ile)
                Vector3 lookDirection = targetWaypoint.position - currentProduct.transform.position;
                lookDirection.y = 0; // Y ekseninde dönmesini engelle (sadece yatayda dönüþ)
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                currentProduct.transform.rotation = Quaternion.Lerp(currentProduct.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Waypoint'e ulaþtýðýnda bir sonraki waypoint'e geç
                if (Vector3.Distance(currentProduct.transform.position, targetWaypoint.position) < 0.1f)
                {
                    currentWaypointIndex++; // Ýndex'i bir sonraki waypoint'e geçmek için artýr

                    // Eðer 1. durma noktasýna geldiysek
                    if (currentWaypointIndex == 5)
                    {
                        yield return StartCoroutine(WaitAndPlayFirstAnimation());
                    }

                    // Eðer 2. durma noktasýna geldiysek
                    if (currentWaypointIndex == 4)
                    {
                        yield return StartCoroutine(WaitAndPlaySecondAnimation());
                    }
                }
            }

            // Bir sonraki kareyi bekle
            yield return null;
        }

        // Ürün EndPoint'e ulaþtýðýnda hareket biter, yeni ürün spawn edilebilir
        isProductMoving = false;
    }

    IEnumerator WaitAndPlayFirstAnimation()
    {
        isWaiting = true;

        // Ýlk animasyonu oynat
        robotArmAnimation.Play("RobotArm");

        // Ýlk bekleme süresince animasyon oynar
        yield return new WaitForSeconds(waitTimeFirst);

        // Hareket devam eder
        isWaiting = false;
    }

    IEnumerator WaitAndPlaySecondAnimation()
    {
        isWaiting = true;

        // Ýkinci animasyonu oynat
        secondAnimation.Play("Line06");

        // Ýkinci bekleme süresince animasyon oynar
        yield return new WaitForSeconds(waitTimeSecond);

        // Hareket devam eder
        isWaiting = false;
    }

    void UpdateProductCounter()
    {
        // Text'in null olup olmadýðýný kontrol et
        if (productCounterText != null)
        {
            productCounterText.text = "Üretilen Ürün: " + totalProductsProduced.ToString();
        }
    }
}
