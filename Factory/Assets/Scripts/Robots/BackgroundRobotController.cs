using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class SimpleWorkerController : MonoBehaviour
{
    [SerializeField] private Transform[] _moveTargets; // 6 Kaynak noktası (0-5) ve 7. nokta final ürün
    [SerializeField] private FactoryResourcesController _resourceController; // Kaynakları kontrol eden controller
    [SerializeField] private GameObject _carryCube; // Taşıdığı kaynak objesi
    [SerializeField] private TextMeshProUGUI _statusText; // Durum mesajı göstergesi
    [SerializeField] private float pickupDistance = 2f; // Ürün alma mesafesi
    [SerializeField] private float dropDistance = 2f;   // Ürün bırakma mesafesi

    // Animasyon parametreleri
    public Animator anim;

    private NavMeshAgent navMeshAgent;
    private string workerTag; // İşçinin tag'i (isci1, isci2, ...)
    private int assignedPointIndex; // İşçinin atanmış olduğu kaynak noktası
    private Transform currentTarget; // Şu anki hedefi
    private bool hasResource = false; // İşçinin kaynak taşıyıp taşımadığı
    public Transform chiefEngineerFinalPoint; // Başmühendisin final ürünü götüreceği nokta
    private Transform chiefStartPosition; // Başmühendisin başlangıç pozisyonu
    private bool isFinalProductDelivered = false; // Final ürün teslim edildi mi?

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.acceleration = 8f; // Hızlanmayı artır
        navMeshAgent.stoppingDistance = 0.5f; // Durma mesafesini küçült
        workerTag = gameObject.tag;
        assignedPointIndex = GetAssignedPointIndex(workerTag); // İşçinin gideceği kaynak noktası
        chiefStartPosition = transform; // Başmühendis için başlangıç pozisyonunu sakla
    }

    private void Update()
    {
        // Eğer başmühendis ise ve tüm kaynaklar toplandıysa final ürünü alıp teslimat noktasına götür
        if (workerTag == "basMuhendis" && _resourceController.AllResourcesCollected() && !isFinalProductDelivered)
        {
            HandleChiefEngineer(); // Başmühendis final ürünü taşıma işlemini başlat
            return;
        }

        // Eğer kaynak yoksa dur ve Idle animasyonu oynat
        if (!ResourceAvailableAtPoint(assignedPointIndex) && !hasResource)
        {
            StopMoving();
            anim.SetBool("Idle", true);
            anim.SetBool("Walking", false);
            anim.SetBool("CarryMove", false);
            return;
        }

        // Eğer kaynak varsa ve taşıdığı kaynak yoksa yürüyüş animasyonu ile hareket et
        if (ResourceAvailableAtPoint(assignedPointIndex) && !hasResource)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", true);
            anim.SetBool("CarryMove", false);
            MoveToPoint(_moveTargets[assignedPointIndex]);
        }

        // Kaynağa ulaşıldığında kaynağı al ve kaynak taşıma animasyonu başlat
        if (!hasResource && currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= pickupDistance)
        {
            CollectResource();
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", false);
            anim.SetBool("CarryMove", true);
        }

        // Eğer işçi kaynak taşıyorsa, kaynak toplama noktasına git
        if (hasResource)
        {
            MoveToPoint(_moveTargets[6]); // 7. nokta, final ürün
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", false);
            anim.SetBool("CarryMove", true);
        }

        // Final noktaya ulaştığında kaynağı bırak
        if (hasResource && currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= dropDistance)
        {
            DropResource();
            anim.SetBool("Idle", true);
            anim.SetBool("Walking", false);
            anim.SetBool("CarryMove", false);
        }
    }

    private void HandleChiefEngineer()
    {
        if (!hasResource)
        {
            // Final ürünü al
            CollectResource();
            MoveToPoint(chiefEngineerFinalPoint); // Final ürünü teslimat noktasına götür
            _statusText.text = "Başmühendis final ürünü alıyor.";
        }
        else if (hasResource && Vector3.Distance(transform.position, chiefEngineerFinalPoint.position) <= dropDistance)
        {
            DropResource();
            isFinalProductDelivered = true;
            _statusText.text = "Başmühendis final ürünü teslim etti.";
            MoveToPoint(chiefStartPosition); // Başlangıç pozisyonuna dön
        }
    }

    private int GetAssignedPointIndex(string tag)
    {
        switch (tag)
        {
            case "isci1": return 0;
            case "isci2": return 1;
            case "isci3": return 2;
            case "isci4": return 3;
            case "isci5": return 4;
            case "isci6": return 5;
            case "basMuhendis": return 6; // Başmühendis final ürün işlemi yapacak
            default: return -1; // Geçersiz tag
        }
    }

    private void MoveToPoint(Transform target)
    {
        if (target != null)
        {
            currentTarget = target;
            navMeshAgent.SetDestination(target.position);
            _statusText.text = $"{workerTag} çalışıyor...";
        }
    }

    private void StopMoving()
    {
        navMeshAgent.isStopped = true;
        _statusText.text = $"{workerTag} kaynak bekliyor";
    }

    private bool ResourceAvailableAtPoint(int pointIndex)
    {
        return _resourceController.HasResourcesForWorker(workerTag);
    }

    private void CollectResource()
    {
        /*
        public static int steel = 100;
    public static int copper = 100;
    public static int plastic = 100;
    public static int glass = 100;
    public static int rubber = 100;
    public static int silicon = 100;
        */
        if (gameObject.tag == "isci1")
        {
            FactoryResourcesController.steel -= 1;
        }
        else if (gameObject.tag == "isci2")
        {
            FactoryResourcesController.copper -= 1;
        }
        else if (gameObject.tag == "isci3")
        {
            FactoryResourcesController.plastic -= 1;
        }
        else if (gameObject.tag == "isci4")
        {
            FactoryResourcesController.glass -= 1;
        }
        else if (gameObject.tag == "isci5")
        {
            FactoryResourcesController.rubber -= 1;
        }
        else if (gameObject.tag == "isci6")
        {
            FactoryResourcesController.silicon -= 1;
        }
        hasResource = true;
        _carryCube.SetActive(true); // Kaynak alındı, taşıma başlasın
        _statusText.text = $"{workerTag} kaynakları topladı";
        currentTarget = null; // Yeni hedef final nokta olacak
    }

    private void DropResource()
    {
        hasResource = false;
        _carryCube.SetActive(false); // Kaynak bırakıldı
        _statusText.text = $"{workerTag} kaynakları bıraktı";
        //Teslim edilen siparişleri burada göster 
        currentTarget = null; // Tekrar kaynağa geri dönecek
    }
}
